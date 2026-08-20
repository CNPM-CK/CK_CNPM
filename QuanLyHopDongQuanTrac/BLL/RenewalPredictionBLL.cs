using DAL;
using DTO.AI;
using DTO.ChatDTO;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BLL
{
    public sealed class RenewalPredictionBLL
    {
        private static readonly Regex IdentifierPattern = new Regex(
            @"\b(?:KH|HD)\s*[-_]?\s*\d{1,12}\b",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private readonly RenewalPredictionDAL _dal = new RenewalPredictionDAL();
        private readonly RenewalMlApiService _mlService = new RenewalMlApiService();
        private readonly GeminiRenewalExplanationService _explanationService =
            new GeminiRenewalExplanationService();

        public static bool IsRenewalIntent(string? message)
        {
            string normalized = RemoveDiacritics(message ?? string.Empty).ToLowerInvariant();
            return normalized.Contains("tai ky", StringComparison.Ordinal) ||
                   normalized.Contains("tai ki", StringComparison.Ordinal) ||
                   normalized.Contains("gia han", StringComparison.Ordinal) ||
                   normalized.Contains("ky tiep", StringComparison.Ordinal) ||
                   normalized.Contains("renewal", StringComparison.Ordinal) ||
                   normalized.Contains("renew contract", StringComparison.Ordinal);
        }

        public async Task<AIPollutionChatResultDTO> BuildChatReplyAsync(string userMessage)
        {
            string? identifier = TryExtractIdentifier(userMessage);
            if (identifier == null)
            {
                return Reply(
                    "Bạn hãy cung cấp mã khách hàng hoặc mã hợp đồng, ví dụ: " +
                    "“Dự báo khả năng tái ký của KH001” hoặc “Phân tích tái ký HD001”.");
            }

            RenewalSnapshotAvailabilityDTO availability;
            try
            {
                availability = await _dal.BuildOfficialSnapshotAsync(identifier, DateTime.Today);
            }
            catch (SqlException exception)
            {
                Debug.WriteLine($"Renewal snapshot SQL failed: {exception}");
                return Reply(
                    "Không thể đọc dữ liệu hợp đồng từ SQL Server. " +
                    "Hãy kiểm tra connection string, database và quyền đăng nhập.");
            }
            catch (Exception exception) when (exception is JsonException or InvalidDataException)
            {
                Debug.WriteLine($"Renewal snapshot audit data failed: {exception}");
                return Reply(
                    "Snapshot T-60 đã lưu không hợp lệ. Hãy kiểm tra bảng audit dự báo tái ký.");
            }
            if (availability.Status != RenewalSnapshotStatus.Ready || availability.Snapshot == null)
            {
                return Reply(availability.Message);
            }

            RenewalSnapshotDTO snapshot = availability.Snapshot;
            RenewalMlPredictionDTO prediction;
            try
            {
                prediction = await _mlService.PredictAsync(snapshot);
            }
            catch (Exception exception) when (
                exception is HttpRequestException or TaskCanceledException or
                InvalidOperationException or JsonException)
            {
                Debug.WriteLine($"Renewal ML API failed: {exception}");
                return Reply(
                    "Không thể gọi dịch vụ ML dự báo tái ký lúc này. " +
                    "Hãy kiểm tra ai-taiky-api đang chạy và cấu hình RenewalPrediction:BaseUrl.");
            }

            string explanation;
            try
            {
                explanation = await _explanationService.ExplainAsync(snapshot, prediction);
            }
            catch (Exception exception) when (
                exception is HttpRequestException or TaskCanceledException or
                InvalidOperationException or JsonException)
            {
                Debug.WriteLine($"Gemini renewal explanation failed: {exception}");
                explanation = BuildDeterministicExplanation(snapshot);
            }

            RenewalForecastDTO forecast = new RenewalForecastDTO
            {
                Snapshot = snapshot,
                Prediction = prediction,
                Explanation = explanation
            };

            try
            {
                await _dal.SaveForecastAsync(forecast);
                forecast.Persisted = true;
            }
            catch (Exception exception)
            {
                // Lưu audit không được làm mất kết quả dự báo đã tính thành công.
                Debug.WriteLine($"Renewal prediction persistence failed: {exception}");
            }

            return Reply(FormatReply(forecast));
        }

        private static string? TryExtractIdentifier(string message)
        {
            Match match = IdentifierPattern.Match(message ?? string.Empty);
            if (!match.Success)
            {
                return null;
            }

            return Regex.Replace(match.Value.ToUpperInvariant(), @"[\s_-]", string.Empty);
        }

        private static string FormatReply(RenewalForecastDTO forecast)
        {
            RenewalSnapshotDTO snapshot = forecast.Snapshot;
            RenewalMlPredictionDTO prediction = forecast.Prediction;
            string evidence = prediction.EvidenceLevel switch
            {
                "high" => "cao",
                "medium" => "trung bình",
                _ => "thấp"
            };
            string conclusion = prediction.PredictedRenewal
                ? "Có xu hướng tái ký"
                : "Chưa đạt ngưỡng tái ký";

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Dự báo tái ký cho hợp đồng {snapshot.ContractId}:");
            builder.AppendLine(
                $"- Xác suất từ ML: {prediction.RenewalProbability:P1}");
            builder.AppendLine(
                $"- Ngưỡng quyết định: {prediction.DecisionThreshold:P1}");
            builder.AppendLine($"- Kết luận của model: {conclusion}");
            builder.AppendLine($"- Mức bằng chứng: {evidence}");
            builder.AppendLine($"- Snapshot: {snapshot.SnapshotDate} (T-60)");
            builder.AppendLine();
            builder.AppendLine(forecast.Explanation.Trim());

            if (prediction.IsColdStart)
            {
                builder.AppendLine();
                builder.AppendLine(
                    "Lưu ý cold-start: khách hàng chưa có đủ lịch sử hợp đồng, " +
                    "nên cần đối chiếu thêm thông tin từ nhân viên phụ trách.");
            }

            builder.AppendLine();
            builder.Append(
                "Lưu ý: đây là hỗ trợ quyết định từ mô hình huấn luyện trên dữ liệu synthetic, " +
                "không thay thế đánh giá nghiệp vụ.");
            return builder.ToString();
        }

        private static string BuildDeterministicExplanation(RenewalSnapshotDTO snapshot)
        {
            List<string> signals = new List<string>();
            if (snapshot.CompletionRateToCutoff.HasValue)
            {
                signals.Add(snapshot.CompletionRateToCutoff.Value >= 0.8
                    ? "tiến độ hoàn thành đến T-60 đang ở mức tốt"
                    : "tiến độ hoàn thành đến T-60 còn hạn chế");
            }
            if (snapshot.OnTimeRateCompleted.HasValue)
            {
                signals.Add(snapshot.OnTimeRateCompleted.Value >= 0.8
                    ? "phần lớn kết quả được trả đúng hạn"
                    : "tỷ lệ trả kết quả đúng hạn cần được cải thiện");
            }
            if (snapshot.OpenOverdueRoundsAtCutoff > 0)
            {
                signals.Add($"còn {snapshot.OpenOverdueRoundsAtCutoff} đợt quá hạn tại cutoff");
            }
            if (snapshot.PreviousContractCount == 0)
            {
                signals.Add("chưa có lịch sử hợp đồng để đối chiếu");
            }

            string evidence = signals.Count == 0
                ? "chưa có đủ KPI vận hành để giải thích sâu"
                : string.Join("; ", signals);
            return
                $"Giải thích: {evidence}.\n" +
                "Đề xuất: rà soát các đợt còn chậm, xác nhận mức độ hài lòng và liên hệ khách hàng trước khi chuẩn bị đề nghị tái ký.";
        }

        private static AIPollutionChatResultDTO Reply(string text) =>
            new AIPollutionChatResultDTO { ReplyText = text };

        private static string RemoveDiacritics(string value)
        {
            string decomposed = value.Normalize(NormalizationForm.FormD);
            StringBuilder builder = new StringBuilder(decomposed.Length);
            foreach (char character in decomposed)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(character);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(character == 'đ' ? 'd' : character == 'Đ' ? 'D' : character);
                }
            }
            return builder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}

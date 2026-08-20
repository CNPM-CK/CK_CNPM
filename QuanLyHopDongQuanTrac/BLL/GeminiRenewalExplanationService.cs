using DTO;
using DTO.AI;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace BLL
{
    public sealed class GeminiRenewalExplanationService
    {
        private const string DefaultModel = "gemini-3.5-flash-lite";

        private static readonly HttpClient HttpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        public async Task<string> ExplainAsync(
            RenewalSnapshotDTO snapshot,
            RenewalMlPredictionDTO prediction)
        {
            string apiKey = AppConfig.GetRequired("Gemini:ApiKey");
            string model = AppConfig.GetOptional("Gemini:Model") ?? DefaultModel;
            string url =
                $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";

            var requestBody = new
            {
                system_instruction = new
                {
                    role = "system",
                    parts = new[]
                    {
                        new
                        {
                            text =
                                "Bạn là trợ lý phân tích nghiệp vụ hợp đồng quan trắc môi trường của ECOS. " +
                                "Mô hình ML đã quyết định xác suất và kết luận; bạn chỉ được giải thích các KPI đầu vào. " +
                                "Không tính lại, không sửa, không làm tròn lại và không tạo thêm xác suất. " +
                                "Không khẳng định quan hệ nhân quả. Trả lời tiếng Việt, văn bản thuần, ngắn gọn."
                        }
                    }
                },
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = BuildPrompt(snapshot, prediction) }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.2,
                    maxOutputTokens = 450
                }
            };

            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("x-goog-api-key", apiKey);
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await HttpClient.SendAsync(request);
            string responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Gemini HTTP {(int)response.StatusCode} {response.ReasonPhrase}.");
            }

            using JsonDocument document = JsonDocument.Parse(responseJson);
            if (!document.RootElement.TryGetProperty("candidates", out JsonElement candidates) ||
                candidates.GetArrayLength() == 0 ||
                !candidates[0].TryGetProperty("content", out JsonElement content) ||
                !content.TryGetProperty("parts", out JsonElement parts) ||
                parts.GetArrayLength() == 0 ||
                !parts[0].TryGetProperty("text", out JsonElement textElement))
            {
                throw new InvalidOperationException("Gemini không trả về phần giải thích hợp lệ.");
            }

            string explanation = textElement.GetString()?.Trim()
                ?? throw new InvalidOperationException("Phần giải thích của Gemini bị rỗng.");
            if (explanation.Contains('%') ||
                explanation.Contains("xác suất", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Gemini đã cố đưa thêm nhận định xác suất ngoài kết quả ML.");
            }

            return explanation;
        }

        private static string BuildPrompt(
            RenewalSnapshotDTO snapshot,
            RenewalMlPredictionDTO prediction)
        {
            string Format(double? value, string format = "0.##") =>
                value.HasValue
                    ? value.Value.ToString(format, CultureInfo.InvariantCulture)
                    : "không có";

            return $@"
Hãy giải thích kết quả đã cố định cho hợp đồng {snapshot.ContractId}, khách hàng {snapshot.CustomerId}.
Snapshot chính thức: {snapshot.SnapshotDate} (đúng T-60).
Kết quả ML cố định, chỉ dùng làm ngữ cảnh và tuyệt đối không nhắc lại con số: predicted_renewal={prediction.PredictedRenewal}; evidence_level={prediction.EvidenceLevel}; cold_start={prediction.IsColdStart}.

KPI tại T-60:
- Hoàn thành: {snapshot.RoundsCompletedByCutoff}/{snapshot.RoundsDueByCutoff} đợt đến hạn; tỷ lệ {Format(snapshot.CompletionRateToCutoff, "0.000")}.
- Đúng hạn: {Format(snapshot.OnTimeRateCompleted, "0.000")}; trễ trung bình: {Format(snapshot.AverageDelayDays)} ngày; số đợt đang quá hạn: {snapshot.OpenOverdueRoundsAtCutoff}.
- 90 ngày gần nhất: {snapshot.Recent90dCompletedRounds} đợt hoàn thành; trễ trung bình {Format(snapshot.Recent90dAverageDelayDays)} ngày.
- Lịch sử: {snapshot.PreviousContractCount} hợp đồng trước; tỷ lệ hoàn thành {Format(snapshot.HistoricalCompletionRate, "0.000")}; tỷ lệ đúng hạn {Format(snapshot.HistoricalOnTimeRate, "0.000")}; tỷ lệ tái ký lịch sử {Format(snapshot.HistoricalRenewalRate, "0.000")}.

Chỉ viết đúng hai phần, mỗi phần tối đa 3 dòng:
Giải thích: nêu tín hiệu thuận lợi và rủi ro có căn cứ trực tiếp từ KPI.
Đề xuất: nêu hành động nghiệp vụ cụ thể trước khi nhân viên liên hệ tái ký.
Không nhắc lại xác suất, không thêm số liệu ngoài danh sách, không dùng Markdown hoặc ký tự *.";
        }
    }
}

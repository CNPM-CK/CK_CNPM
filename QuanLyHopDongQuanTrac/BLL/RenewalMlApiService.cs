using DTO;
using DTO.AI;
using System.Text;
using System.Text.Json;

namespace BLL
{
    public sealed class RenewalMlApiService
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;

        public RenewalMlApiService()
        {
            string baseUrl = AppConfig.GetOptional("RenewalPrediction:BaseUrl")
                ?? "http://127.0.0.1:8000/";
            string normalizedBaseUrl = baseUrl.TrimEnd('/') + "/";
            if (!Uri.TryCreate(normalizedBaseUrl, UriKind.Absolute, out Uri? baseAddress))
            {
                throw new InvalidOperationException(
                    $"RenewalPrediction:BaseUrl không hợp lệ: '{baseUrl}'.");
            }
            if (baseAddress.Scheme != Uri.UriSchemeHttp &&
                baseAddress.Scheme != Uri.UriSchemeHttps)
            {
                throw new InvalidOperationException(
                    "RenewalPrediction:BaseUrl chỉ hỗ trợ giao thức HTTP hoặc HTTPS.");
            }

            int timeoutSeconds = 15;
            string? configuredTimeout = AppConfig.GetOptional("RenewalPrediction:TimeoutSeconds");
            if (!string.IsNullOrWhiteSpace(configuredTimeout) &&
                (!int.TryParse(configuredTimeout, out timeoutSeconds) || timeoutSeconds <= 0))
            {
                throw new InvalidOperationException(
                    "RenewalPrediction:TimeoutSeconds phải là số nguyên dương.");
            }

            _httpClient = new HttpClient
            {
                BaseAddress = baseAddress,
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };
        }

        public async Task<RenewalMlPredictionDTO> PredictAsync(RenewalSnapshotDTO snapshot)
        {
            string payload = JsonSerializer.Serialize(snapshot, JsonOptions);
            using StringContent content = new StringContent(
                payload, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _httpClient.PostAsync(
                "v1/predictions/renewal", content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"ML API HTTP {(int)response.StatusCode} {response.ReasonPhrase}: " +
                    Truncate(responseBody, 1200));
            }

            RenewalMlPredictionDTO? prediction = JsonSerializer.Deserialize<RenewalMlPredictionDTO>(
                responseBody, JsonOptions);
            if (prediction == null || string.IsNullOrWhiteSpace(prediction.ModelVersion))
            {
                throw new InvalidOperationException("ML API trả về dữ liệu dự báo không hợp lệ.");
            }

            if (!double.IsFinite(prediction.RenewalProbability) ||
                prediction.RenewalProbability is < 0 or > 1 ||
                !double.IsFinite(prediction.DecisionThreshold) ||
                prediction.DecisionThreshold is < 0 or > 1 ||
                prediction.EvidenceLevel is not ("low" or "medium" or "high") ||
                !prediction.DecisionSupportOnly)
            {
                throw new InvalidOperationException(
                    "ML API trả về xác suất, ngưỡng hoặc mức bằng chứng không hợp lệ.");
            }

            if (!string.Equals(prediction.SnapshotId, snapshot.SnapshotId, StringComparison.Ordinal) ||
                !string.Equals(prediction.CustomerId, snapshot.CustomerId, StringComparison.Ordinal) ||
                !string.Equals(prediction.ContractId, snapshot.ContractId, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    "ML API trả về định danh không khớp với snapshot được gửi.");
            }

            return prediction;
        }

        private static string Truncate(string value, int maximumLength) =>
            value.Length <= maximumLength ? value : value[..maximumLength] + "...";
    }
}

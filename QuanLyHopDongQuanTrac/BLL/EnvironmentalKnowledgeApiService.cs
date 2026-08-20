using DTO;
using DTO.AI;
using System.Text;
using System.Text.Json;

namespace BLL
{
    public sealed class EnvironmentalKnowledgeApiService
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;

        public EnvironmentalKnowledgeApiService()
        {
            string baseUrl = AppConfig.GetOptional("EnvironmentalKnowledge:BaseUrl")
                ?? AppConfig.GetOptional("RenewalPrediction:BaseUrl")
                ?? "http://127.0.0.1:8000/";
            string normalizedBaseUrl = baseUrl.TrimEnd('/') + "/";
            if (!Uri.TryCreate(normalizedBaseUrl, UriKind.Absolute, out Uri? baseAddress) ||
                (baseAddress.Scheme != Uri.UriSchemeHttp &&
                 baseAddress.Scheme != Uri.UriSchemeHttps))
            {
                throw new InvalidOperationException(
                    $"EnvironmentalKnowledge:BaseUrl không hợp lệ: '{baseUrl}'.");
            }

            int timeoutSeconds = 20;
            string? configuredTimeout =
                AppConfig.GetOptional("EnvironmentalKnowledge:TimeoutSeconds");
            if (!string.IsNullOrWhiteSpace(configuredTimeout) &&
                (!int.TryParse(configuredTimeout, out timeoutSeconds) || timeoutSeconds <= 0))
            {
                throw new InvalidOperationException(
                    "EnvironmentalKnowledge:TimeoutSeconds phải là số nguyên dương.");
            }

            _httpClient = new HttpClient
            {
                BaseAddress = baseAddress,
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };
        }

        public async Task<EnvironmentalKnowledgeSearchResponseDTO> SearchAsync(
            string query,
            int topK = 5,
            DateOnly? asOfDate = null)
        {
            var request = new EnvironmentalKnowledgeSearchRequestDTO
            {
                Query = query,
                TopK = topK,
                AsOfDate = asOfDate?.ToString("yyyy-MM-dd")
            };
            string payload = JsonSerializer.Serialize(request, JsonOptions);
            using StringContent content = new StringContent(
                payload, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _httpClient.PostAsync(
                "v1/knowledge/search", content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Knowledge API HTTP {(int)response.StatusCode} " +
                    $"{response.ReasonPhrase}: {Truncate(responseBody, 1200)}");
            }

            EnvironmentalKnowledgeSearchResponseDTO? result =
                JsonSerializer.Deserialize<EnvironmentalKnowledgeSearchResponseDTO>(
                    responseBody, JsonOptions);
            Validate(result, query);
            return result!;
        }

        private static void Validate(
            EnvironmentalKnowledgeSearchResponseDTO? response,
            string expectedQuery)
        {
            if (response == null ||
                !string.Equals(response.Query, expectedQuery, StringComparison.Ordinal) ||
                string.IsNullOrWhiteSpace(response.IndexVersion) ||
                response.RetrievalMode is not ("sparse" or "hybrid") ||
                response.Results.Count > 10)
            {
                throw new InvalidOperationException(
                    "Knowledge API trả về metadata không hợp lệ.");
            }

            var citationIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (EnvironmentalKnowledgeCitationDTO citation in response.Results)
            {
                bool validUrl = Uri.TryCreate(
                    citation.CanonicalUrl, UriKind.Absolute, out Uri? canonicalUri) &&
                    canonicalUri.Scheme == Uri.UriSchemeHttps;
                if (string.IsNullOrWhiteSpace(citation.CitationId) ||
                    !citationIds.Add(citation.CitationId) ||
                    string.IsNullOrWhiteSpace(citation.SourceId) ||
                    string.IsNullOrWhiteSpace(citation.DocumentNumber) ||
                    string.IsNullOrWhiteSpace(citation.Excerpt) ||
                    citation.Excerpt.Length > 4_000 ||
                    !double.IsFinite(citation.Score) ||
                    citation.Score is < 0 or > 1 ||
                    !validUrl)
                {
                    throw new InvalidOperationException(
                        "Knowledge API trả về trích dẫn không hợp lệ.");
                }
            }
        }

        private static string Truncate(string value, int maximumLength) =>
            value.Length <= maximumLength ? value : value[..maximumLength] + "...";
    }
}

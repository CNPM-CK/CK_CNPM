using System.Text.Json.Serialization;

namespace DTO.AI
{
    public sealed class EnvironmentalKnowledgeSearchRequestDTO
    {
        [JsonPropertyName("query")]
        public string Query { get; set; } = string.Empty;

        [JsonPropertyName("top_k")]
        public int TopK { get; set; } = 5;

        [JsonPropertyName("as_of_date")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AsOfDate { get; set; }

        [JsonPropertyName("topics")]
        public List<string> Topics { get; set; } = new List<string>();

        [JsonPropertyName("source_ids")]
        public List<string> SourceIds { get; set; } = new List<string>();
    }

    public sealed class EnvironmentalKnowledgeCitationDTO
    {
        [JsonPropertyName("citation_id")]
        public string CitationId { get; set; } = string.Empty;

        [JsonPropertyName("source_id")]
        public string SourceId { get; set; } = string.Empty;

        [JsonPropertyName("document_number")]
        public string DocumentNumber { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("section")]
        public string? Section { get; set; }

        [JsonPropertyName("page")]
        public int? Page { get; set; }

        [JsonPropertyName("excerpt")]
        public string Excerpt { get; set; } = string.Empty;

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("canonical_url")]
        public string CanonicalUrl { get; set; } = string.Empty;

        [JsonPropertyName("download_url")]
        public string? DownloadUrl { get; set; }

        [JsonPropertyName("effective_from")]
        public string? EffectiveFrom { get; set; }

        [JsonPropertyName("effective_to")]
        public string? EffectiveTo { get; set; }

        [JsonPropertyName("legal_status")]
        public string LegalStatus { get; set; } = string.Empty;

        [JsonPropertyName("content_sha256")]
        public string ContentSha256 { get; set; } = string.Empty;
    }

    public sealed class EnvironmentalKnowledgeSearchResponseDTO
    {
        [JsonPropertyName("query")]
        public string Query { get; set; } = string.Empty;

        [JsonPropertyName("as_of_date")]
        public string AsOfDate { get; set; } = string.Empty;

        [JsonPropertyName("retrieval_mode")]
        public string RetrievalMode { get; set; } = string.Empty;

        [JsonPropertyName("index_version")]
        public string IndexVersion { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public List<EnvironmentalKnowledgeCitationDTO> Results { get; set; } =
            new List<EnvironmentalKnowledgeCitationDTO>();

        [JsonPropertyName("grounded_context")]
        public string GroundedContext { get; set; } = string.Empty;

        [JsonPropertyName("warning")]
        public string Warning { get; set; } = string.Empty;
    }
}

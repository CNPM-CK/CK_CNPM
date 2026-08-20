using System.Text.Json.Serialization;

namespace DTO.AI
{
    public enum RenewalSnapshotStatus
    {
        Ready,
        TooEarly,
        NotFound,
        InvalidData
    }

    public sealed class RenewalSnapshotAvailabilityDTO
    {
        public RenewalSnapshotStatus Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime? EligibleDate { get; set; }
        public RenewalSnapshotDTO? Snapshot { get; set; }
    }

    public sealed class RenewalSnapshotDTO
    {
        [JsonIgnore]
        public string CustomerName { get; set; } = string.Empty;

        [JsonPropertyName("snapshot_id")]
        public string SnapshotId { get; set; } = string.Empty;

        [JsonPropertyName("customer_id")]
        public string CustomerId { get; set; } = string.Empty;

        [JsonPropertyName("contract_id")]
        public string ContractId { get; set; } = string.Empty;

        [JsonPropertyName("snapshot_date")]
        public string SnapshotDate { get; set; } = string.Empty;

        [JsonPropertyName("contract_end_date")]
        public string ContractEndDate { get; set; } = string.Empty;

        [JsonPropertyName("latest_observed_event_date")]
        public string? LatestObservedEventDate { get; set; }

        [JsonPropertyName("frequency_code")]
        public string FrequencyCode { get; set; } = string.Empty;

        [JsonPropertyName("contract_sequence_number")]
        public int ContractSequenceNumber { get; set; }

        [JsonPropertyName("contract_duration_months")]
        public int ContractDurationMonths { get; set; }

        [JsonPropertyName("days_observed_current_contract")]
        public int DaysObservedCurrentContract { get; set; }

        [JsonPropertyName("expected_rounds_total")]
        public int ExpectedRoundsTotal { get; set; }

        [JsonPropertyName("rounds_due_by_cutoff")]
        public int RoundsDueByCutoff { get; set; }

        [JsonPropertyName("rounds_completed_by_cutoff")]
        public int RoundsCompletedByCutoff { get; set; }

        [JsonPropertyName("open_overdue_rounds_at_cutoff")]
        public int OpenOverdueRoundsAtCutoff { get; set; }

        [JsonPropertyName("completion_rate_to_cutoff")]
        public double? CompletionRateToCutoff { get; set; }

        [JsonPropertyName("on_time_rate_completed")]
        public double? OnTimeRateCompleted { get; set; }

        [JsonPropertyName("average_delay_days")]
        public double? AverageDelayDays { get; set; }

        [JsonPropertyName("maximum_delay_days")]
        public double? MaximumDelayDays { get; set; }

        [JsonPropertyName("average_processing_days")]
        public double? AverageProcessingDays { get; set; }

        [JsonPropertyName("maximum_processing_days")]
        public double? MaximumProcessingDays { get; set; }

        [JsonPropertyName("recent_90d_completed_rounds")]
        public int Recent90dCompletedRounds { get; set; }

        [JsonPropertyName("recent_90d_average_delay_days")]
        public double? Recent90dAverageDelayDays { get; set; }

        [JsonPropertyName("has_customer_history")]
        public int HasCustomerHistory { get; set; }

        [JsonPropertyName("previous_contract_count")]
        public int PreviousContractCount { get; set; }

        [JsonPropertyName("relationship_tenure_days")]
        public int RelationshipTenureDays { get; set; }

        [JsonPropertyName("days_since_previous_contract_end")]
        public int? DaysSincePreviousContractEnd { get; set; }

        [JsonPropertyName("historical_rounds_completed")]
        public int HistoricalRoundsCompleted { get; set; }

        [JsonPropertyName("historical_completion_rate")]
        public double? HistoricalCompletionRate { get; set; }

        [JsonPropertyName("historical_on_time_rate")]
        public double? HistoricalOnTimeRate { get; set; }

        [JsonPropertyName("historical_average_delay_days")]
        public double? HistoricalAverageDelayDays { get; set; }

        [JsonPropertyName("historical_average_processing_days")]
        public double? HistoricalAverageProcessingDays { get; set; }

        [JsonPropertyName("historical_renewal_rate")]
        public double? HistoricalRenewalRate { get; set; }

        [JsonPropertyName("current_metrics_available")]
        public int CurrentMetricsAvailable { get; set; }

        [JsonPropertyName("historical_metrics_available")]
        public int HistoricalMetricsAvailable { get; set; }
    }

    public sealed class RenewalMlPredictionDTO
    {
        [JsonPropertyName("snapshot_id")]
        public string SnapshotId { get; set; } = string.Empty;

        [JsonPropertyName("customer_id")]
        public string CustomerId { get; set; } = string.Empty;

        [JsonPropertyName("contract_id")]
        public string ContractId { get; set; } = string.Empty;

        [JsonPropertyName("renewal_probability")]
        public double RenewalProbability { get; set; }

        [JsonPropertyName("predicted_renewal")]
        public bool PredictedRenewal { get; set; }

        [JsonPropertyName("decision_threshold")]
        public double DecisionThreshold { get; set; }

        [JsonPropertyName("is_cold_start")]
        public bool IsColdStart { get; set; }

        [JsonPropertyName("evidence_level")]
        public string EvidenceLevel { get; set; } = string.Empty;

        [JsonPropertyName("model_version")]
        public string ModelVersion { get; set; } = string.Empty;

        [JsonPropertyName("decision_support_only")]
        public bool DecisionSupportOnly { get; set; }

        [JsonPropertyName("warning")]
        public string? Warning { get; set; }
    }

    public sealed class RenewalForecastDTO
    {
        public RenewalSnapshotDTO Snapshot { get; set; } = new RenewalSnapshotDTO();
        public RenewalMlPredictionDTO Prediction { get; set; } = new RenewalMlPredictionDTO();
        public string Explanation { get; set; } = string.Empty;
        public bool Persisted { get; set; }
    }
}

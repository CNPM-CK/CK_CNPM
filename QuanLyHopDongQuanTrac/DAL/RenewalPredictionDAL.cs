using DTO.AI;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Text.Json;

namespace DAL
{
    public sealed class RenewalPredictionDAL
    {
        public async Task<RenewalSnapshotAvailabilityDTO> BuildOfficialSnapshotAsync(
            string identifier,
            DateTime asOfDate)
        {
            string normalizedIdentifier = (identifier ?? string.Empty).Trim().ToUpperInvariant();
            if (string.IsNullOrWhiteSpace(normalizedIdentifier))
            {
                return Invalid("Mã khách hàng hoặc hợp đồng không được để trống.");
            }

            await using SqlConnection connection = SqlConnectionData.Connect();
            await connection.OpenAsync();

            ContractRow? current = await FindContractAsync(
                connection,
                normalizedIdentifier,
                normalizedIdentifier.StartsWith("HD", StringComparison.OrdinalIgnoreCase));

            if (current == null)
            {
                return new RenewalSnapshotAvailabilityDTO
                {
                    Status = RenewalSnapshotStatus.NotFound,
                    Message = $"Không tìm thấy khách hàng hoặc hợp đồng '{normalizedIdentifier}'."
                };
            }

            DateTime cutoff = current.EndDate.Date.AddDays(-60);
            if (asOfDate.Date < cutoff)
            {
                return new RenewalSnapshotAvailabilityDTO
                {
                    Status = RenewalSnapshotStatus.TooEarly,
                    EligibleDate = cutoff,
                    Message =
                        $"Hợp đồng {current.ContractId} chưa đến thời điểm dự báo chính thức. " +
                        $"Snapshot T-60 sẽ được mở từ {cutoff:dd/MM/yyyy}."
                };
            }

            if (cutoff < current.StartDate.Date)
            {
                return Invalid(
                    $"Hợp đồng {current.ContractId} có thời hạn ngắn hơn 60 ngày nên không thể tạo snapshot T-60.");
            }

            if (DurationMonths(current.StartDate, current.EndDate) > 120)
            {
                return Invalid(
                    $"Hợp đồng {current.ContractId} có thời hạn vượt quá giới hạn 120 tháng của model.");
            }

            RenewalSnapshotDTO? frozenSnapshot = await TryLoadFrozenSnapshotAsync(
                connection, current.CustomerId, current.ContractId, cutoff);
            if (frozenSnapshot != null)
            {
                return new RenewalSnapshotAvailabilityDTO
                {
                    Status = RenewalSnapshotStatus.Ready,
                    EligibleDate = cutoff,
                    Message = "Đã tải snapshot T-60 được khóa từ lần dự báo đầu tiên.",
                    Snapshot = frozenSnapshot
                };
            }

            List<ContractRow> contracts = await LoadCustomerContractsAsync(
                connection, current.CustomerId, cutoff);
            List<MonitoringRoundRow> rounds = await LoadCustomerRoundsAsync(
                connection, current.CustomerId);

            RenewalSnapshotDTO snapshot = BuildSnapshot(current, contracts, rounds, cutoff);
            return new RenewalSnapshotAvailabilityDTO
            {
                Status = RenewalSnapshotStatus.Ready,
                EligibleDate = cutoff,
                Message = "Snapshot T-60 sẵn sàng.",
                Snapshot = snapshot
            };
        }

        public async Task SaveForecastAsync(RenewalForecastDTO forecast)
        {
            string snapshotJson = JsonSerializer.Serialize(forecast.Snapshot);
            await using SqlConnection connection = SqlConnectionData.Connect();
            await connection.OpenAsync();
            await using SqlCommand command = new SqlCommand(
                "dbo.sp_AI_Renewal_SavePrediction", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@snapshotId", SqlDbType.VarChar, 100).Value = forecast.Snapshot.SnapshotId;
            command.Parameters.Add("@maKH", SqlDbType.VarChar, 15).Value = forecast.Snapshot.CustomerId;
            command.Parameters.Add("@maHD", SqlDbType.VarChar, 15).Value = forecast.Snapshot.ContractId;
            command.Parameters.Add("@snapshotDate", SqlDbType.Date).Value = DateTime.ParseExact(
                forecast.Snapshot.SnapshotDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            command.Parameters.Add("@modelVersion", SqlDbType.VarChar, 120).Value = forecast.Prediction.ModelVersion;
            command.Parameters.Add("@renewalProbability", SqlDbType.Float).Value = forecast.Prediction.RenewalProbability;
            command.Parameters.Add("@predictedRenewal", SqlDbType.Bit).Value = forecast.Prediction.PredictedRenewal;
            command.Parameters.Add("@decisionThreshold", SqlDbType.Float).Value = forecast.Prediction.DecisionThreshold;
            command.Parameters.Add("@isColdStart", SqlDbType.Bit).Value = forecast.Prediction.IsColdStart;
            command.Parameters.Add("@evidenceLevel", SqlDbType.VarChar, 20).Value = forecast.Prediction.EvidenceLevel;
            command.Parameters.Add("@snapshotJson", SqlDbType.NVarChar, -1).Value = snapshotJson;
            command.Parameters.Add("@explanation", SqlDbType.NVarChar, -1).Value = forecast.Explanation;
            await command.ExecuteNonQueryAsync();
        }

        private static async Task<RenewalSnapshotDTO?> TryLoadFrozenSnapshotAsync(
            SqlConnection connection,
            string customerId,
            string contractId,
            DateTime cutoff)
        {
            await using (SqlCommand objectCommand = new SqlCommand(
                "SELECT OBJECT_ID(N'dbo.AI_RenewalPrediction', N'U');", connection))
            {
                object? objectId = await objectCommand.ExecuteScalarAsync();
                if (objectId == null || objectId == DBNull.Value)
                {
                    return null;
                }
            }

            const string sql = @"
SELECT TOP (1) snapshotJson
FROM dbo.AI_RenewalPrediction
WHERE maKH = @maKH AND maHD = @maHD AND snapshotDate = @snapshotDate
ORDER BY predictedAtUtc, predictionId;";
            await using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.Add("@maKH", SqlDbType.VarChar, 15).Value = customerId;
            command.Parameters.Add("@maHD", SqlDbType.VarChar, 15).Value = contractId;
            command.Parameters.Add("@snapshotDate", SqlDbType.Date).Value = cutoff;
            object? value = await command.ExecuteScalarAsync();
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return null;
            }

            RenewalSnapshotDTO? snapshot = JsonSerializer.Deserialize<RenewalSnapshotDTO>(
                value.ToString()!);
            if (snapshot == null ||
                !string.Equals(snapshot.CustomerId, customerId, StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(snapshot.ContractId, contractId, StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(snapshot.SnapshotDate, cutoff.ToString("yyyy-MM-dd"), StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    $"Snapshot đã lưu của hợp đồng {contractId} không hợp lệ.");
            }

            return snapshot;
        }

        private static RenewalSnapshotDTO BuildSnapshot(
            ContractRow current,
            List<ContractRow> allContracts,
            List<MonitoringRoundRow> allRounds,
            DateTime cutoff)
        {
            List<ContractRow> previousContracts = allContracts
                .Where(contract =>
                    contract.StartDate < current.StartDate ||
                    (contract.StartDate == current.StartDate &&
                     string.Compare(contract.ContractId, current.ContractId, StringComparison.Ordinal) < 0))
                .OrderBy(contract => contract.StartDate)
                .ThenBy(contract => contract.ContractId)
                .ToList();

            HashSet<string> previousIds = previousContracts
                .Select(contract => contract.ContractId)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            List<MonitoringRoundRow> currentRounds = allRounds
                .Where(round => string.Equals(round.ContractId, current.ContractId, StringComparison.OrdinalIgnoreCase))
                .ToList();
            List<MonitoringRoundRow> historicalRounds = allRounds
                .Where(round => previousIds.Contains(round.ContractId))
                .ToList();

            List<MonitoringRoundRow> due = currentRounds
                .Where(round => round.PlannedResultDate.HasValue && round.PlannedResultDate.Value.Date <= cutoff)
                .ToList();
            List<MonitoringRoundRow> completed = currentRounds
                .Where(round => round.ActualResultDate.HasValue && round.ActualResultDate.Value.Date <= cutoff)
                .ToList();
            List<MonitoringRoundRow> completedWithPlan = completed
                .Where(round => round.PlannedResultDate.HasValue)
                .ToList();
            List<MonitoringRoundRow> completedWithProcessingDates = completed
                .Where(round => round.ActualStartDate.HasValue && round.ActualResultDate.HasValue)
                .ToList();
            DateTime recentStart = cutoff.AddDays(-90);
            List<MonitoringRoundRow> recentCompleted = completed
                .Where(round => round.ActualResultDate!.Value.Date >= recentStart)
                .ToList();
            List<MonitoringRoundRow> historicalCompleted = historicalRounds
                .Where(round => round.ActualResultDate.HasValue && round.ActualResultDate.Value.Date <= cutoff)
                .ToList();
            List<MonitoringRoundRow> historicalCompletedWithPlan = historicalCompleted
                .Where(round => round.PlannedResultDate.HasValue)
                .ToList();
            List<MonitoringRoundRow> historicalCompletedWithProcessingDates = historicalCompleted
                .Where(round => round.ActualStartDate.HasValue && round.ActualResultDate.HasValue)
                .ToList();
            int historicalDue = historicalRounds.Count(round =>
                round.PlannedResultDate.HasValue && round.PlannedResultDate.Value.Date <= cutoff);

            List<double> delays = completedWithPlan.Select(DelayDays).ToList();
            List<double> recentDelays = recentCompleted
                .Where(round => round.PlannedResultDate.HasValue)
                .Select(DelayDays)
                .ToList();
            List<double> processingDays = completedWithProcessingDates.Select(ProcessingDays).ToList();
            List<double> historicalDelays = historicalCompletedWithPlan.Select(DelayDays).ToList();
            List<double> historicalProcessing = historicalCompletedWithProcessingDates
                .Select(ProcessingDays)
                .ToList();

            // Không dùng số row hiện có làm kế hoạch tổng vì các row có thể được
            // tạo sau T-60. Kế hoạch tổng được suy ra từ điều khoản hợp đồng.
            int expectedRounds = ExpectedRoundCount(
                current.FrequencyCode, current.StartDate, current.EndDate);

            List<int> historicalOutcomes = new List<int>();
            foreach (ContractRow previous in previousContracts.Where(contract =>
                         contract.EndDate.Date.AddDays(90) <= cutoff))
            {
                ContractRow? next = allContracts
                    .Where(contract =>
                        contract.StartDate > previous.StartDate ||
                        (contract.StartDate == previous.StartDate &&
                         string.Compare(contract.ContractId, previous.ContractId, StringComparison.Ordinal) > 0))
                    .OrderBy(contract => contract.StartDate)
                    .ThenBy(contract => contract.ContractId)
                    .FirstOrDefault();
                historicalOutcomes.Add(
                    next != null && next.StartDate.Date <= previous.EndDate.Date.AddDays(90) ? 1 : 0);
            }

            DateTime firstStart = previousContracts.Count > 0
                ? previousContracts.Min(contract => contract.StartDate)
                : current.StartDate;
            ContractRow? latestPrevious = previousContracts
                .OrderByDescending(contract => contract.EndDate)
                .FirstOrDefault();

            int openOverdue = due.Count(round =>
                !round.ActualResultDate.HasValue || round.ActualResultDate.Value.Date > cutoff);
            double? completionRate = due.Count == 0
                ? 0.0
                : Clamp01((double)completed.Count / due.Count);
            DateTime? latestObservedEventDate = completed
                .Where(round => round.ActualResultDate.HasValue)
                .Select(round => round.ActualResultDate)
                .Max();

            return new RenewalSnapshotDTO
            {
                CustomerName = current.CustomerName,
                SnapshotId = $"SNAP_{current.ContractId}",
                CustomerId = current.CustomerId,
                ContractId = current.ContractId,
                SnapshotDate = cutoff.ToString("yyyy-MM-dd"),
                ContractEndDate = current.EndDate.ToString("yyyy-MM-dd"),
                LatestObservedEventDate = latestObservedEventDate?.ToString("yyyy-MM-dd"),
                FrequencyCode = NormalizeFrequency(current.FrequencyCode),
                ContractSequenceNumber = previousContracts.Count + 1,
                ContractDurationMonths = DurationMonths(current.StartDate, current.EndDate),
                DaysObservedCurrentContract = Math.Max(0, (cutoff - current.StartDate.Date).Days),
                ExpectedRoundsTotal = Math.Max(
                    expectedRounds, Math.Max(due.Count, completed.Count)),
                RoundsDueByCutoff = due.Count,
                RoundsCompletedByCutoff = completed.Count,
                OpenOverdueRoundsAtCutoff = openOverdue,
                CompletionRateToCutoff = completionRate,
                OnTimeRateCompleted = completedWithPlan.Count == 0
                    ? null
                    : (double)completedWithPlan.Count(round =>
                        round.ActualResultDate!.Value.Date <= round.PlannedResultDate!.Value.Date) /
                      completedWithPlan.Count,
                AverageDelayDays = AverageOrNull(delays),
                MaximumDelayDays = MaxOrNull(delays),
                AverageProcessingDays = AverageOrNull(processingDays),
                MaximumProcessingDays = MaxOrNull(processingDays),
                Recent90dCompletedRounds = recentCompleted.Count,
                Recent90dAverageDelayDays = AverageOrNull(recentDelays),
                HasCustomerHistory = previousContracts.Count > 0 ? 1 : 0,
                PreviousContractCount = previousContracts.Count,
                RelationshipTenureDays = Math.Max(0, (cutoff - firstStart.Date).Days),
                DaysSincePreviousContractEnd = latestPrevious == null
                    ? null
                    : Math.Max(0, (current.StartDate.Date - latestPrevious.EndDate.Date).Days),
                HistoricalRoundsCompleted = historicalCompleted.Count,
                HistoricalCompletionRate = historicalDue == 0
                    ? null
                    : Clamp01((double)historicalCompleted.Count / historicalDue),
                HistoricalOnTimeRate = historicalCompletedWithPlan.Count == 0
                    ? null
                    : (double)historicalCompletedWithPlan.Count(round =>
                        round.ActualResultDate!.Value.Date <= round.PlannedResultDate!.Value.Date) /
                      historicalCompletedWithPlan.Count,
                HistoricalAverageDelayDays = AverageOrNull(historicalDelays),
                HistoricalAverageProcessingDays = AverageOrNull(historicalProcessing),
                HistoricalRenewalRate = AverageOrNull(historicalOutcomes.Select(value => (double)value).ToList()),
                CurrentMetricsAvailable = completed.Count > 0 ? 1 : 0,
                HistoricalMetricsAvailable = historicalCompleted.Count > 0 ? 1 : 0
            };
        }

        private static async Task<ContractRow?> FindContractAsync(
            SqlConnection connection,
            string identifier,
            bool isContractId)
        {
            string whereClause = isContractId ? "h.maHD = @identifier" : "h.maKH = @identifier";
            string sql = $@"
SELECT TOP (1)
    h.maHD,
    h.maKH,
    kh.tenDoanhNghiep,
    h.ngayKy,
    h.ngayKetThucHD,
    h.tanSuatQuanTrac
FROM dbo.HopDong h
INNER JOIN dbo.KhachHang kh ON kh.maKH = h.maKH
WHERE {whereClause}
ORDER BY h.ngayKetThucHD DESC, h.ngayKy DESC, h.maHD DESC;";

            await using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.Add("@identifier", SqlDbType.VarChar, 15).Value = identifier;
            await using SqlDataReader reader = await command.ExecuteReaderAsync();
            return await reader.ReadAsync() ? ReadContract(reader) : null;
        }

        private static async Task<List<ContractRow>> LoadCustomerContractsAsync(
            SqlConnection connection,
            string customerId,
            DateTime cutoff)
        {
            const string sql = @"
SELECT h.maHD, h.maKH, kh.tenDoanhNghiep, h.ngayKy, h.ngayKetThucHD, h.tanSuatQuanTrac
FROM dbo.HopDong h
INNER JOIN dbo.KhachHang kh ON kh.maKH = h.maKH
WHERE h.maKH = @maKH AND h.ngayKy <= @cutoff
ORDER BY h.ngayKy, h.maHD;";
            List<ContractRow> result = new List<ContractRow>();
            await using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.Add("@maKH", SqlDbType.VarChar, 15).Value = customerId;
            command.Parameters.Add("@cutoff", SqlDbType.Date).Value = cutoff;
            await using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(ReadContract(reader));
            }
            return result;
        }

        private static async Task<List<MonitoringRoundRow>> LoadCustomerRoundsAsync(
            SqlConnection connection,
            string customerId)
        {
            const string sql = @"
SELECT d.maDot, d.maHD, d.ngayBatDau, d.ngayDuKien, d.ngayTraKQ
FROM dbo.DotQuanTrac d
INNER JOIN dbo.HopDong h ON h.maHD = d.maHD
WHERE h.maKH = @maKH;";
            List<MonitoringRoundRow> result = new List<MonitoringRoundRow>();
            await using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.Add("@maKH", SqlDbType.VarChar, 15).Value = customerId;
            await using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new MonitoringRoundRow
                {
                    RoundId = reader["maDot"].ToString() ?? string.Empty,
                    ContractId = reader["maHD"].ToString() ?? string.Empty,
                    ActualStartDate = ReadNullableDate(reader, "ngayBatDau"),
                    PlannedResultDate = ReadNullableDate(reader, "ngayDuKien"),
                    ActualResultDate = ReadNullableDate(reader, "ngayTraKQ")
                });
            }
            return result;
        }

        private static ContractRow ReadContract(SqlDataReader reader) => new ContractRow
        {
            ContractId = reader["maHD"].ToString() ?? string.Empty,
            CustomerId = reader["maKH"].ToString() ?? string.Empty,
            CustomerName = reader["tenDoanhNghiep"].ToString() ?? string.Empty,
            StartDate = Convert.ToDateTime(reader["ngayKy"]),
            EndDate = Convert.ToDateTime(reader["ngayKetThucHD"]),
            FrequencyCode = reader["tanSuatQuanTrac"].ToString() ?? string.Empty
        };

        private static DateTime? ReadNullableDate(SqlDataReader reader, string column) =>
            reader[column] == DBNull.Value ? null : Convert.ToDateTime(reader[column]);

        private static double DelayDays(MonitoringRoundRow round) => Math.Max(
            0,
            (round.ActualResultDate!.Value.Date - round.PlannedResultDate!.Value.Date).TotalDays);

        private static double ProcessingDays(MonitoringRoundRow round) => Math.Max(
            0,
            (round.ActualResultDate!.Value.Date - round.ActualStartDate!.Value.Date).TotalDays);

        private static double? AverageOrNull(List<double> values) =>
            values.Count == 0 ? null : values.Average();

        private static double? MaxOrNull(List<double> values) =>
            values.Count == 0 ? null : values.Max();

        private static double Clamp01(double value) => Math.Max(0.0, Math.Min(1.0, value));

        private static int DurationMonths(DateTime start, DateTime end)
        {
            DateTime exclusiveEnd = end.Date.AddDays(1);
            int months = (exclusiveEnd.Year - start.Year) * 12 + exclusiveEnd.Month - start.Month;
            return Math.Max(1, months);
        }

        private static int ExpectedRoundCount(string frequencyCode, DateTime start, DateTime end)
        {
            int duration = DurationMonths(start, end);
            return NormalizeFrequency(frequencyCode) switch
            {
                "TSQT02" => Math.Max(1, (int)Math.Ceiling(duration / 6.0)),
                "TSQT03" => Math.Max(1, (int)Math.Ceiling(duration / 3.0)),
                _ => 1
            };
        }

        private static string NormalizeFrequency(string value)
        {
            string normalized = (value ?? string.Empty).Trim().ToUpperInvariant();
            return normalized is "TSQT01" or "TSQT02" or "TSQT03" ? normalized : "TSQT01";
        }

        private static RenewalSnapshotAvailabilityDTO Invalid(string message) => new RenewalSnapshotAvailabilityDTO
        {
            Status = RenewalSnapshotStatus.InvalidData,
            Message = message
        };

        private sealed class ContractRow
        {
            public string ContractId { get; set; } = string.Empty;
            public string CustomerId { get; set; } = string.Empty;
            public string CustomerName { get; set; } = string.Empty;
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string FrequencyCode { get; set; } = string.Empty;
        }

        private sealed class MonitoringRoundRow
        {
            public string RoundId { get; set; } = string.Empty;
            public string ContractId { get; set; } = string.Empty;
            public DateTime? ActualStartDate { get; set; }
            public DateTime? PlannedResultDate { get; set; }
            public DateTime? ActualResultDate { get; set; }
        }
    }
}

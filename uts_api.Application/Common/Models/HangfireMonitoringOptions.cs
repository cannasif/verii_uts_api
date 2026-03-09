namespace uts_api.Application.Common.Models;

public sealed class HangfireMonitoringOptions
{
    public const string SectionName = "HangfireMonitoring";

    public bool EnableFailureSqlLog { get; set; } = true;
    public int FinalRetryCountThreshold { get; set; } = 3;
    public List<string> CriticalJobs { get; set; } = [];
}

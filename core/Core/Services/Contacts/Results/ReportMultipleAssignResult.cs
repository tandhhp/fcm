namespace Waffle.Core.Services.Contacts.Results;

public class ReportMultipleAssignResult
{
    public string? TeamName { get; set; }
    public List<ReportMultipleAssignData> Data { get; set; } = [];
    public int TotalAssigned => Data.Sum(x => x.TotalAssigned);
    public int TotalUsingAssigned => Data.Sum(x => x.TotalUsingAssigned);
    public int TotalRemainAssigned => Data.Sum(x => x.TotalRemainAssigned);
}

public class ReportMultipleAssignData
{
    public string? SourceName { get; set; }
    public string? TeleName { get; set; }
    public int TotalAssigned { get; set; }
    public int TotalUsingAssigned { get; set; }
    public int TotalRemainAssigned { get; set; }
}
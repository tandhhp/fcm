namespace Waffle.Core.Services.Calls.Results;

public class TeleReportResult
{
    public string? CCM { get; set; }
    public List<TeleReportTeamResult>? Teams { get; set; }
}

public class TeleReportTeamResult
{
    public string? Team { get; set; }
    public List<TeleReportUserResult>? Members { get; set; }
}

public class TeleReportUserResult
{
    public int TalkTime { get; set; }
    public int Confirm1 { get; set; }
    public int Confirm2 { get; set; }
    public int Consider { get; set; }
    public int NotSure { get; set; }
}
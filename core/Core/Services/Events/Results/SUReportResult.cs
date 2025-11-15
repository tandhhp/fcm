namespace Waffle.Core.Services.Events.Results;

public class SUReportResult
{
    public string? SalesManagerName { get; set; }
    public List<SUSalesReport> SalesReports { get; set; } = [];
}

public class SUSalesReport
{
    public string? SalesName { get; set; }
    public List<SUAttendance> Attendances { get; set; } = [];
}

public class SUAttendance
{
    public int AttendanceId { get; set; }
    public float Count { get; set; }
    public string Name { get; set; } = default!;
}

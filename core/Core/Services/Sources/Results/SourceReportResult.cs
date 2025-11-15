namespace Waffle.Core.Services.Sources.Results;

public class SourceReportResult
{
    public string SourceName { get; set; } = default!;
    public int ContactCount { get; set; }
    public int CallCount { get; set; }
}

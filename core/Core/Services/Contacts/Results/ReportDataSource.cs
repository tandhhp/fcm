namespace Waffle.Core.Services.Contacts.Results;

public class ReportDataSource
{
    public string? SourceGroup { get; set; }
    public List<SourceName>? SourceNames { get; set; }
}

public class SourceName
{
    public string? Name { get; set; }
    public int ContactImport { get; set; }
    public int ContactForStartCase { get; set; }
    public int Total { get; set; }
}
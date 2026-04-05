namespace Waffle.Core.Services.Contacts.Args;

public class ContactAssignSourceArgs
{
    public Guid TelesalesId { get; set; }
    public int NumberOfContact { get; set; }
    public int SourceId { get; set; }
}

public class ReportDataSourceFilterOptions
{
    public int? TeamId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? SourceId { get; set; }
}
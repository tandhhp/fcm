using Waffle.Entities.Contacts;

namespace Waffle.Models.Filters;

public class ContactFilterOptions : FilterOptions
{
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? IsBooked { get; set; }
    public bool? Confirm1 { get; set; }
    public Confirm2Status? Confirm2Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? CallStatusId { get; set; }
    public string? Note { get; set; }
    public string? Job { get; set; }
    public string? Age { get; set; }
    public string? ExtraStatus { get; set; }
    public int? SourceId { get; set; }
    public int? TypeOfDataId { get; set; }
    public SourceType? SourceType { get; set; }
    public Guid? TelesalesId { get; set; }
}

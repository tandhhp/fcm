using Waffle.Entities;

namespace Waffle.Core.Services.Leads.Results;

public class LeadExportCheckinResult
{
    public DateTime EventDate { get; set; }
    public string? EventName { get; set; }
    public string? SourceName { get; set; }
    public string? KeyInName { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhoneNumber { get; set; }
    public string? CustomerIdNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? AttendanceName { get; set; }
    public string? TableName { get; set; }
    public string? SalesName { get; set; }
    public string? ToName { get; set; }
    public string? ContractCode { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? AmountPaid { get; set; }
    public string? SalesManagerName { get; set; }
    public Guid? EventId { get; set; }
    public LeadStatus? Status { get; set; }
    public string? TeamKeyIn { get; set; }
    public string? DOS { get; set; }
}

namespace Waffle.Core.Services.Leads.Args;

public class LeadFeedbackCreateArgs
{
    public Guid LeadId { get; set; }
    public string? FinancialSituation { get; set; }
    public int InterestLevel { get; set; }
    public string? RejectReason { get; set; }
    public TimeSpan? CheckinTime { get; set; }
    public TimeSpan? CheckoutTime { get; set; }
    public int? JobKindId { get; set; }
    public int? TableId { get; set; }
    public Guid? SalesId { get; set; }
    public Guid? VoucherId { get; set; }
    public int? SourceId { get; set; }
    public string? IdentityNumber { get; set; }
    public Guid? ToById { get; set; }
    public int? TransportId { get; set; }
}

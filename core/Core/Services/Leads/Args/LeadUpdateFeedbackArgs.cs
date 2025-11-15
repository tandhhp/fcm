using Waffle.Entities;

namespace Waffle.Core.Services.Leads.Args;

public class LeadUpdateFeedbackArgs : BaseEntity
{
    public string Name { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
    public bool? Gender { get; set; }
    public Guid? ToById { get; set; }
    public int? TransportId { get; set; }
    public string? Address { get; set; }
    public Guid? SalesId { get; set; }
    public int? TableId { get; set; }
    public int? AttendanceId { get; set; }
    public string? Note { get; set; }
    public int? JobKindId { get; set; }
    public int InterestLevel { get; set; }
    public string? FinancialSituation { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? Voucher1Id { get; set; }
    public Guid? Voucher2Id { get; set; }
    public string? IdentityNumber { get; set; }
}

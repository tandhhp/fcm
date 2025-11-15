using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waffle.Entities;

public class UserTopup
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    [Column(TypeName = "money")]
    public decimal Amount { get; set; }
    public TopupStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? CardHolderId { get; set; }
    public string? Note { get; set; }
    public DateTime? DirectorApprovedDate { get; set; }
    public DateTime? AccountantApprovedDate { get; set; }
    public Guid? DirectorId { get; set; }
    public Guid? AccountantId { get; set; }
    public Guid? SmId { get; set; }
    public Guid? DosId { get; set; }
    public TopupType Type { get; set; }
    public Guid? LeadId { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ContractCode { get; set; }
    public string? Evidence { get; set; }
}

public enum TopupStatus
{
    [Display(Name = "Chờ duyệt")]
    Pending,
    [Display(Name = "GĐ duyệt")]
    DirectorApproved,
    [Display(Name = "KT duyệt")]
    AccountantApproved,
    [Display(Name = "Từ chối")]
    Rejected
}
public enum TopupType
{
    Topup,
    [Display(Name = "Công nợ")]
    Debt,
    Event
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Waffle.Entities.Contacts;
using Waffle.Entities.Contracts;

namespace Waffle.Entities;

public class Lead : BaseEntity
{
    [StringLength(256)]
    public string Name { get; set; } = default!;
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
    [StringLength(256)]
    public string? Email { get; set; }
    [StringLength(2048)]
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    [ForeignKey(nameof(Sales))]
    public Guid? SalesId { get; set; }
    public LeadStatus Status { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime EventDate { get; set; }
    [Comment("DDCN")]
    [StringLength(12)]
    public string? IdentityNumber { get; set; }
    [ForeignKey(nameof(Branch))]
    public int BranchId { get; set; }
    public bool? Gender { get; set; }
    [ForeignKey(nameof(Telesales))]
    public Guid? TelesaleId { get; set; }
    public string? Note { get; set; }
    public Guid CreatedBy { get; set; }
    [ForeignKey(nameof(Event))]
    public Guid EventId { get; set; }
    public int? SourceId { get; set; }
    [ForeignKey(nameof(Attendance))]
    public int? AttendanceId { get; set; }
    public Guid? ToById { get; set; }
    public bool Duplicated { get; set; }
    [ForeignKey(nameof(Voucher1))]
    public Guid? Voucher1Id { get; set; }
    [ForeignKey(nameof(Voucher2))]
    public Guid? Voucher2Id { get; set; }

    public virtual Event? Event { get; set; }
    public virtual Branch? Branch { get; set; }
    public virtual Source? Source { get; set; }
    public ApplicationUser? Sales { get; set; }
    public ApplicationUser? Telesales { get; set; }
    public virtual Attendance? Attendance { get; set; }
    public virtual Voucher? Voucher1 { get; set; }
    public virtual Voucher? Voucher2 { get; set; }
    public ICollection<SubLead>? SubLeads { get; set; }
    public ICollection<LeadFeedback>? Feedbacks { get; set; }
    public ICollection<LeadHistory>? Histories { get; set; }
}

public enum LeadStatus
{
    [Display(Name = "Chờ duyệt")]
    Pending,
    [Display(Name = "Đã duyệt")]
    Approved,
    [Display(Name = "Check-in")]
    Checkin,
    [Display(Name = "Chốt deal")]
    LeadAccept,
    [Display(Name = "Từ chối")]
    LeadReject,
    [Display(Name = "Mời lại")]
    ReInvite
}

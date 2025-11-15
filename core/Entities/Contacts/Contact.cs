using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Waffle.Entities.Users;

namespace Waffle.Entities.Contacts;

public class Contact : AuditEntity
{
    [StringLength(450)]
    public string Name { get; set; } = default!;
    [StringLength(450)]
    public string? Email { get; set; }
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
    [StringLength(1000)]
    public string? Note { get; set; }
    [StringLength(500)]
    public string? Address { get; set; }
    public ContactStatus Status { get; set; }
    [ForeignKey(nameof(Transport))]
    public int? TransportId { get; set; }
    [ForeignKey(nameof(District))]
    public int? DistrictId { get; set; }
    public bool? Gender { get; set; }
    public MarriedStatus? MarriedStatus { get; set; }
    public Guid? UserId { get; set; }
    [ForeignKey(nameof(JobKind))]
    public int? JobKindId { get; set; }
    [ForeignKey(nameof(Source))]
    public int? SourceId { get; set; }
    public bool Confirm1 { get; set; }
    public bool Confirm2 { get; set; }

    public virtual Transport? Transport { get; set; }
    public virtual District? District { get; set; }
    public virtual JobKind? JobKind { get; set; }
    public virtual Source? Source { get; set; }
    public virtual ICollection<ContactActivity>? Activities { get; set; }
    public virtual ICollection<CallHistory>? CallHistories { get; set; }
}

public enum ContactStatus
{
    [Display(Name = "Mới")]
    New,
    [Display(Name = "Danh sách đen")]
    Blacklisted
}

public enum MarriedStatus
{
    [Display(Name = "Độc thân")]
    Single,
    [Display(Name = "Đã kết hôn")]
    Married,
    [Display(Name = "Ly hôn")]
    Divorced,
    [Display(Name = "Góa")]
    Widowed
}

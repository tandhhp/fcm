using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waffle.Entities.Contacts;

public class CallHistory : BaseEntity
{
    [ForeignKey(nameof(Contact))]
    public Guid ContactId { get; set; }
    [ForeignKey(nameof(CallStatus))]
    public int CallStatusId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? Note { get; set; }
    public Guid CreatedBy { get; set; }
    public string? MetaData { get; set; }
    [StringLength(1024)]
    public string? TravelHabit { get; set; }
    [StringLength(256)]
    public string? Age { get; set; }
    [StringLength(512)]
    public string? Job { get; set; }
    [StringLength(256)]
    public string? ExtraStatus { get; set; }
    public DateTime? FollowUpDate { get; set; }

    public virtual Contact? Contact { get; set; }
    public virtual CallStatus? CallStatus { get; set; }
}

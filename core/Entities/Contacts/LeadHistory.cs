using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waffle.Entities.Contacts;

public class LeadHistory : BaseEntity
{
    [ForeignKey(nameof(Lead))]
    public Guid LeadId { get; set; }
    public Guid? SalesId { get; set; }
    public DateTime? EventDate { get; set; }
    public Guid? TelesaleId { get; set; }
    public string? Note { get; set; }
    public int? AttendanceId { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ToById { get; set; }
    public int? TableId { get; set; }
    public TimeSpan? CheckinTime { get; set; }
    public TimeSpan? CheckoutTime { get; set; }
    public Guid? EventId { get; set; }
    public int? TransportId { get; set; }

    public virtual Lead? Lead { get; set; }
}

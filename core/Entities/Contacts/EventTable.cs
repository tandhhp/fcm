using System.ComponentModel.DataAnnotations.Schema;

namespace Waffle.Entities.Contacts;

public class EventTable : BaseEntity
{
    public DateTime EventDate { get; set; }
    [ForeignKey(nameof(Table))]
    public int TableId { get; set; }
    [ForeignKey(nameof(Event))]
    public Guid EventId { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }

    public virtual Table? Table { get; set; }
    public virtual Event? Event { get; set; }
}

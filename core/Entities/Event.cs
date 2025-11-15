using Waffle.Entities.Contacts;

namespace Waffle.Entities;

public class Event : AuditEntity
{
    public string Name { get; set; } = default!;

    public virtual ICollection<Lead>? Leads { get; set; }
    public virtual ICollection<EventTable>? EventTables { get; set; }
}
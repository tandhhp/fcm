using System.ComponentModel.DataAnnotations;
using Waffle.Entities.Contacts;

namespace Waffle.Entities;

public class Event : AuditEntity
{
    public string Name { get; set; } = default!;
    [StringLength(7)]
    public string? Color { get; set; }

    public virtual ICollection<Lead>? Leads { get; set; }
    public virtual ICollection<EventTable>? EventTables { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Waffle.Entities.Contacts;

namespace Waffle.Entities;

public class Room : AuditEntity<int>
{
    [StringLength(256)]
    public string Name { get; set; } = default!;
    [ForeignKey(nameof(Branch))]
    public int BranchId { get; set; }

    public virtual Branch? Branch { get; set; }
    public virtual ICollection<Table>? Tables { get; set; }
}

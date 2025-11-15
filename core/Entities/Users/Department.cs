using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waffle.Entities.Users;

public class Department : BaseEntity<int>
{
    public Guid? LeaderId { get; set; }
    [StringLength(256)]
    public string Name { get; set; } = default!;
    [ForeignKey(nameof(Branch))]
    public int BranchId { get; set; }

    public Branch? Branch { get; set; }
}

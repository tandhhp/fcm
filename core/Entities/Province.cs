using System.ComponentModel.DataAnnotations;

namespace Waffle.Entities;

public class Province : BaseEntity<int>
{
    [StringLength(100)]
    public string Name { get; set; } = default!;

    public virtual ICollection<District>? Districts { get; set; }
}

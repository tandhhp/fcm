using System.ComponentModel.DataAnnotations;

namespace Waffle.Entities.Contacts;

public class Attendance : BaseEntity<int>
{
    [StringLength(256)]
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    public float SuRate { get; set; }
    [StringLength(7)]
    public string? Color { get; set; }

    public ICollection<Lead>? Leads { get; set; }
}

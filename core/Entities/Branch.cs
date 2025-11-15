using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Waffle.Entities.Users;

namespace Waffle.Entities;

public class Branch : BaseEntity<int>
{
    [StringLength(256)]
    public string Name { get; set; } = default!;
    [ForeignKey(nameof(District))]
    public int? DistrictId { get; set; }

    public virtual District? District { get; set; }
    public virtual ICollection<Department>? Departments { get; set; }
    public virtual ICollection<Room>? Rooms { get; set; }
}

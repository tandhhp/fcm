using System.ComponentModel.DataAnnotations;
using Waffle.Entities.Contracts;

namespace Waffle.Entities;

public class Gift : BaseEntity
{
    [StringLength(256)]
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }

    public virtual ICollection<Contract>? Contracts { get; set; }
}

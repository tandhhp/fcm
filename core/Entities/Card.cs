using System.ComponentModel.DataAnnotations;
using Waffle.Entities.Contracts;

namespace Waffle.Entities;

public class Card : BaseEntity
{
    [StringLength(256)]
    public string Code { get; set; } = default!;
    [StringLength(2048)]
    public string? FrontImage { get; set; }
    [StringLength(2048)]
    public string? BackImage { get; set; }
    public string? Content { get; set; }
    [StringLength(256)]
    public string Name { get; set; } = default!;

    public virtual ICollection<Contract>? Contracts { get; set; }
}

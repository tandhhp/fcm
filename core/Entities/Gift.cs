using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waffle.Entities;

public class Gift : BaseEntity
{
    [StringLength(256)]
    public string Name { get; set; } = default!;
    [Column(TypeName = "money")]
    public decimal Amount { get; set; }
    public DateOnly CreatedDate { get; set; }
}

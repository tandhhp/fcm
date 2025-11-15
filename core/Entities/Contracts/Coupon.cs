using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waffle.Entities.Contracts;

public class Coupon : AuditEntity
{
    [StringLength(512)]
    public string Name { get; set; } = default!;
    [Column(TypeName = "money")]
    public decimal Discount { get; set; }
    [ForeignKey(nameof(Contract))]
    public Guid ContractId { get; set; }

    public virtual Contract? Contract { get; set; }
}

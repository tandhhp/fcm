using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Waffle.Entities.Contracts;

namespace Waffle.Entities;

public class Gift : BaseEntity
{
    [ForeignKey(nameof(Contract))]
    public Guid ContractId { get; set; }
    [StringLength(256)]
    public string Name { get; set; } = default!;
    [Column(TypeName = "money")]
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateOnly ExpiredDate { get; set; }
    public Guid UserId { get; set; }

    public virtual Contract? Contract { get; set; }
}

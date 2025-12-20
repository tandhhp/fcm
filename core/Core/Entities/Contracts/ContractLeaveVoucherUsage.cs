using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waffle.Entities.Contracts;

public class ContractLeaveVoucherUsage : AuditEntity
{
    [ForeignKey(nameof(Contract))]
    public Guid ContractId { get; set; }

    [StringLength(256)]
    public string VoucherName { get; set; } = default!;

    public DateTime UsedDate { get; set; }

    public int PeopleCount { get; set; }

    [Column(TypeName = "money")]
    public decimal Amount { get; set; }

    public virtual Contract? Contract { get; set; }
}
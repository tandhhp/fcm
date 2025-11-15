using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Waffle.Entities.Contracts;

namespace Waffle.Entities.Payments;

public class Bill : AuditEntity
{
    [Column(TypeName = "money")]
    public decimal Amount { get; set; }
    [ForeignKey(nameof(Contract))]
    public Guid ContractId { get; set; }
    public string Name { get; set; } = default!;
    [StringLength(2048)]
    public string? Note { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public BillStatus Status { get; set; }
    [StringLength(35)]
    public string BillNumber { get; set; } = default!;

    public Contract? Contract { get; set; }
}

public enum BillStatus
{
    Pending,
    Approved,
    Rejected,
    Cancelled
}

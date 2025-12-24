using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Waffle.Entities.Payments;

namespace Waffle.Entities.Contracts;

public class Contract : AuditEntity
{
    [StringLength(256)]
    public string Code { get; set; } = default!;
    [Column(TypeName = "money")]
    public decimal Amount { get; set; }
    public Guid? SalesId { get; set; }
    [ForeignKey(nameof(Card))]
    public Guid? CardId { get; set; }
    public Guid? ToById { get; set; }
    public int? SourceId { get; set; }
    public Guid? TeamKeyInId { get; set; }
    public Guid? KeyInId { get; set; }
    public Guid LeadId { get; set; }

    public virtual Card? Card { get; set; }

    public virtual ICollection<Invoice>? Invoices { get; set; }
    public virtual ICollection<Bill>? Bills { get; set; }
    public virtual ICollection<Coupon>? Coupons { get; set; }
    public virtual ICollection<Evidence>? Evidences { get; set; }
    public virtual ICollection<Gift>? Gifts { get; set; }
}

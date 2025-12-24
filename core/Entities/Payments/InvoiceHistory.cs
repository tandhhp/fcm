using System.ComponentModel.DataAnnotations.Schema;

namespace Waffle.Entities.Payments;

public class InvoiceHistory : BaseEntity
{
    [ForeignKey(nameof(Invoice))]
    public Guid InvoiceId { get; set; }
    public Guid UserId { get; set; }
    public InvoiceStatus Status { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }

    public Invoice? Invoice { get; set; }
}

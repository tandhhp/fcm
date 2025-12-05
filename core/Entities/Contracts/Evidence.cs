using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waffle.Entities.Contracts;

public class Evidence : BaseEntity
{
    [ForeignKey(nameof(Contract))]
    public Guid ContractId { get; set; }
    [StringLength(2048)]
    public string Url { get; set; } = default!;
    [StringLength(256)]
    public string FileName { get; set; } = default!;
    public DateTime UploadAt { get; set; } = DateTime.UtcNow;
    public Guid UploaderId { get; set; }
    [ForeignKey(nameof(EvidenceType))]
    public int? EvidenceTypeId { get; set; }

    public virtual EvidenceType? EvidenceType { get; set; }
    public virtual Contract? Contract { get; set; }
}

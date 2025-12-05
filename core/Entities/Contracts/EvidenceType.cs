using System.ComponentModel.DataAnnotations;

namespace Waffle.Entities.Contracts;

public class EvidenceType : BaseEntity<int>
{
    [StringLength(256)]
    public string Name { get; set; } = default!;

    public virtual ICollection<Evidence>? Evidences { get; set; }
}

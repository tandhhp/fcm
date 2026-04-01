using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waffle.Entities.Contacts;

public class TypeOfData : BaseEntity<int>
{
    [StringLength(256)]
    public string Name { get; set; } = default!;
    [ForeignKey(nameof(RootSource))]
    public int RootSourceId { get; set; }

    public RootSource? RootSource { get; set; }
}

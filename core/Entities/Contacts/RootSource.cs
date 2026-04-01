using System.ComponentModel.DataAnnotations;

namespace Waffle.Entities.Contacts;

public class RootSource : BaseEntity<int>
{
    [StringLength(256)]
    public string Name { get; set; } = default!;

    public ICollection<TypeOfData>? TypeOfDatas { get; set; }
}

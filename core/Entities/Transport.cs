using System.ComponentModel.DataAnnotations;
using Waffle.Entities.Contacts;

namespace Waffle.Entities;

public class Transport : BaseEntity<int>
{
    [StringLength(450)]
    public string Name { get; set; } = default!;

    public virtual ICollection<Contact>? Contacts { get; set; }
}

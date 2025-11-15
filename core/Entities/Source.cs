using System.ComponentModel.DataAnnotations;
using Waffle.Entities.Contacts;

namespace Waffle.Entities;

public class Source : BaseEntity<int>
{
    [StringLength(256)]
    public string Name { get; set; } = default!;

    public virtual ICollection<Lead>? Leads { get; set; }
    public virtual ICollection<ApplicationUser>? Users { get; set; }
    public virtual ICollection<Contact>? Contacts { get; set; }
}

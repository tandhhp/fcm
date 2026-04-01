using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Waffle.Entities.Contacts;
using Waffle.Entities.Users;

namespace Waffle.Entities;

public class Source : BaseEntity<int>
{
    [StringLength(256)]
    public string Name { get; set; } = default!;
    public bool Overwrite { get; set; }
    public bool Protected { get; set; }
    public int TeamId { get; set; }

    public virtual ICollection<Lead>? Leads { get; set; }
    public virtual ICollection<ApplicationUser>? Users { get; set; }
    public virtual ICollection<Contact>? Contacts { get; set; }
}

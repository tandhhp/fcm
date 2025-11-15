using System.ComponentModel.DataAnnotations;
using Waffle.Entities.Contacts;

namespace Waffle.Entities.Users;

public class JobKind : BaseEntity<int>
{
    [StringLength(256)]
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }

    public virtual ICollection<Contact>? Contacts { get; set; }
    public virtual ICollection<LeadFeedback>? LeadFeedbacks { get; set; }
}

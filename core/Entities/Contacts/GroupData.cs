using System.ComponentModel.DataAnnotations;
using Waffle.Entities.Users;

namespace Waffle.Entities.Contacts;

public class GroupData : BaseEntity<int>
{
    [StringLength(100)]
    public string Name { get; set; } = default!;

    public ICollection<Team>? Teams { get; set; }
}

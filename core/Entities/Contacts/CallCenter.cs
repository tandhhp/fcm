using System.ComponentModel.DataAnnotations;
using Waffle.Entities.Users;

namespace Waffle.Entities.Contacts;

public class CallCenter : BaseEntity<int>
{
    [StringLength(100)]
    public string Name { get; set; } = default!;
    [StringLength(64)]
    public string? Code { get; set; }
    public int SortOrder { get; set; }

    public ICollection<Team>? Teams { get; set; }
}

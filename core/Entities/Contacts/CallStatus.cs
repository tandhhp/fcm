using System.ComponentModel.DataAnnotations;

namespace Waffle.Entities.Contacts;

public class CallStatus : BaseEntity<int>
{
    [StringLength(512)]
    public string Name { get; set; } = default!;
    [StringLength(64)]
    public string? Code { get; set; } = default!;

    public virtual ICollection<CallHistory>? CallHistories { get; set; }
}

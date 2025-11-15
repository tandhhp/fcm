using Waffle.Core.Constants;

namespace Waffle.Entities;

public class UserChange
{
    public Guid Id { get; set; }
    public Guid CurrentId { get; set; }
    public Guid TargetId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsAccept { get; set; }
    /// <summary>
    /// <see cref="RoleName"/>
    /// </summary>
    public string Type { get; set; } = default!;
    public string? Note { get; set; }
}

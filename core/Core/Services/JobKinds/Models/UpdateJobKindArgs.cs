using Waffle.Entities;

namespace Waffle.Core.Services.JobKinds.Models;

public class UpdateJobKindArgs : BaseEntity<int>
{
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }
}

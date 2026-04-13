using Waffle.Models;

namespace Waffle.Core.Services.JobKinds.Models;

public class CreateJobKindArgs
{
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }
}

public class JobKindFilterOptions : FilterOptions
{
    public string? Name { get; set; }
}
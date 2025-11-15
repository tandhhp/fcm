using Waffle.Models;

namespace Waffle.Core.Services.Teams.Models;

public class TeamFilterOptions : FilterOptions
{
    public int? DepartmentId { get; set; }
    public string? Name { get; set; }
}

public class UserTeamFilterOptions : FilterOptions
{
    public string? Name { get; set; }
    public int TeamId { get; set; }
}
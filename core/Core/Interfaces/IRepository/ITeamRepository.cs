using Waffle.Core.Services.Teams.Models;
using Waffle.Entities.Users;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository;

public interface ITeamRepository : IAsyncRepository<Team>
{
    Task<bool> ExistsAsync(string name, int departmentId);
    Task<ListResult<object>> ListAsync(TeamFilterOptions filterOptions);
    Task<object?> OptionsAsync(TeamSelectOptions selectOptions);
    Task<ListResult<object>> UsersAsync(UserTeamFilterOptions filterOptions);
}

using Waffle.Core.Services.Teams.Args;
using Waffle.Core.Services.Teams.Models;
using Waffle.Entities.Users;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService;

public interface ITeamService
{
    Task<TResult> CreateAsync(CreateTeamArgs args);
    Task<TResult> DeleteAsync(int id);
    Task<ListResult<object>> ListAsync(TeamFilterOptions filterOptions);
    Task<object?> OptionsAsync(TeamSelectOptions selectOptions);
    Task<TResult> UpdateAsync(UpdateTeamArgs args);
    Task<ListResult<object>> UsersAsync(UserTeamFilterOptions filterOptions);
    Task<Team?> FindAsync(int id);
    Task<TResult> AddUserAsync(TeamUserAddArgs args);
    Task<TResult> RemoveUserAsync(Guid id);
    Task<TResult<object>> DetailAsync(int id);
}

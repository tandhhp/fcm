using Waffle.Models;
using Waffle.Core.Services.Teams.Models;
using Waffle.Entities.Contacts;

namespace Waffle.Core.Services.Teams.Interfaces;

public interface IGroupDataService
{
    Task<TResult> CreateAsync(CreateGroupDataArgs args);
    Task<TResult> UpdateAsync(UpdateGroupDataArgs args);
    Task<TResult> DeleteAsync(int id);
    Task<TResult<object>> DetailAsync(int id);
    Task<GroupData?> FindAsync(int id);
    Task<ListResult<object>> ListAsync(GroupDataFilterOptions filterOptions);
    Task<object> GetOptionsAsync(SelectOptions selectOptions);
}

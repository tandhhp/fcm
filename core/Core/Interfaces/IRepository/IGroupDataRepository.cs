using Waffle.Entities.Contacts;
using Waffle.Core.Services.Teams.Models;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository;

public interface IGroupDataRepository : IAsyncRepository<GroupData>
{
    Task<bool> ExistsAsync(string name, int? excludedId = null);
    Task<bool> HasTeamAsync(int id);
    Task<ListResult<object>> ListAsync(GroupDataFilterOptions filterOptions);
    Task<object> GetOptionsAsync(SelectOptions selectOptions);
}

using Waffle.Core.Services.JobKinds.Models;
using Waffle.Entities.Users;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository;

public interface IJobKindRepository : IAsyncRepository<JobKind>
{
    Task<bool> IsUsedAsync(int id);
    Task<ListResult<object>> ListAsync(JobKindFilterOptions filterOptions);
    Task<object?> OptionsAsync();
}

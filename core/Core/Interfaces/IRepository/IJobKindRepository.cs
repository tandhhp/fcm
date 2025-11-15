using Waffle.Entities.Users;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository;

public interface IJobKindRepository : IAsyncRepository<JobKind>
{
    Task<bool> IsUsedAsync(int id);
    Task<ListResult<object>> ListAsync(FilterOptions filterOptions);
    Task<object?> OptionsAsync();
}

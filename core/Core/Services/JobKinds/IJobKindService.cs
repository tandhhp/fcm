
using Waffle.Core.Services.JobKinds.Models;
using Waffle.Models;

namespace Waffle.Core.Services.JobKinds;

public interface IJobKindService
{
    Task<TResult> CreateAsync(CreateJobKindArgs args);
    Task<TResult> DeleteAsync(int id);
    Task<TResult<object?>> GetAsync(int id);
    Task<ListResult<object>> ListAsync(FilterOptions filterOptions);
    Task<object?> OptionsAsync();
    Task<TResult> UpdateAsync(UpdateJobKindArgs args);
}

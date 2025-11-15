using Waffle.Core.Services.Calls.Args;
using Waffle.Core.Services.Calls.Models;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService.Calls;

public interface ICallStatusService
{
    Task<TResult> CreateAsync(CallStatusCreateArgs args);
    Task<TResult> DeleteAsync(int id);
    Task<ListResult<object>> ListAsync(CallStatusFilterOptions filterOptions);
    Task<object> OptionsAsync(SelectOptions options);
    Task<TResult> UpdateAsync(CallStatusUpdateArgs args);
}

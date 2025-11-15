using Waffle.Core.Services.Events.Models;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService.Events;

public interface IGiftService
{
    Task<TResult> CreateAsync(GiftCreateArgs args);
    Task<TResult> DeleteAsync(Guid id);
    Task<TResult<object>> DetailAsync(Guid id);
    Task<object?> GetOptionsAsync();
    Task<ListResult<object>> ListAsync(GiftFilterOptions filterOptions);
    Task<TResult> UpdateAsync(GiftUpdateArgs args);
}

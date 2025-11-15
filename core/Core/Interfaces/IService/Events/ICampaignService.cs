using Waffle.Core.Services.Events.Models;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService.Events;

public interface ICampaignService
{
    Task<TResult> CreateAsync(CampaignCreateArgs args);
    Task<TResult> DeleteAsync(int id);
    Task<Campaign?> FindAsync(int id);
    Task<TResult<object>> GetAsync(int id);
    Task<ListResult<object>> ListAsync(CampaignFilter filter);
    Task<object> OptionsAsync(SelectFilterOptions filterOptions);
    Task<TResult> UpdateAsync(CampaignUpdateArgs args);
}

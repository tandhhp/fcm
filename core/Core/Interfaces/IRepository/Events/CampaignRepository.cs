using Waffle.Core.Services.Events.Models;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Events;

public interface ICampaignRepository : IAsyncRepository<Campaign>
{
    Task<bool> HasVoucherAsync(int id);
    Task<ListResult<object>> ListAsync(CampaignFilter filter);
    Task<object> OptionsAsync(SelectFilterOptions filterOptions);
}

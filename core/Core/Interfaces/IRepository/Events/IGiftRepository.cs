using Waffle.Core.Services.Events.Models;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Events;

public interface IGiftRepository : IAsyncRepository<Gift>
{
    Task<object?> GetOptionsAsync();
    Task<ListResult<object>> ListAsync(GiftFilterOptions filterOptions);
}

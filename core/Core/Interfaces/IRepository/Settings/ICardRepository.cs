using Waffle.Core.Services.Cards.Filters;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Settings;

public interface ICardRepository : IAsyncRepository<Card>
{
    Task<object> GetOptionsAsync(SelectOptions selectOptions);
    Task<ListResult<object>> ListAsync(CardFilterOptions filterOptions);
}

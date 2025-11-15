using Waffle.Core.Services.Cards.Args;
using Waffle.Core.Services.Cards.Filters;
using Waffle.Models;

namespace Waffle.Core.Services.Cards;

public interface ICardService
{
    Task<TResult> CreateAsync(CardCreateArgs args);
    Task<TResult> DeleteAsync(Guid id);
    Task<TResult<object>> DetailAsync(Guid id);
    Task<ListResult<object>> ListAsync(CardFilterOptions filterOptions);
    Task<object> OptionsAsync(SelectOptions selectOptions);
    Task<TResult> UpdateAsync(CardUpdateArgs args);
}

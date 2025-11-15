using Waffle.Core.Services.Events.Args;
using Waffle.Core.Services.Events.Filters;
using Waffle.Core.Services.Events.Models;
using Waffle.Core.Services.Events.Results;
using Waffle.Core.Services.Leads.Filters;
using Waffle.Core.Services.Tables.Filters;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Events;

public interface IEventRepository : IAsyncRepository<Event>
{
    Task<TResult> CreateContractAsync(Lead lead, string contractCode, decimal amount, Guid? cardId);
    Task<ListResult<object>> GetListAsync(EventFilterOptions filterOptions);
    Task<object> KeyInOptionsAsync(KeyInSelectOptions selectOptions);
    Task<object> OptionsAsync();
    Task<List<SUReportResult>> SuReportAsync(SUFilterOptions filterOptions);
    Task<object?> TableOptionsAsync(AllTableFilterOptions filterOptions);
    Task<object> ToOptionsAsync(SelectOptions selectOptions);
}

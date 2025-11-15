using Waffle.Core.Services.Events.Args;
using Waffle.Core.Services.Events.Filters;
using Waffle.Core.Services.Events.Models;
using Waffle.Core.Services.Events.Results;
using Waffle.Core.Services.Leads.Filters;
using Waffle.Core.Services.Tables.Filters;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService.Events;

public interface IEventService
{
    Task<object?> ListSaleRevenueAsync(SaleRevenueFilterOptions filterOptions);
    Task<ListResult<object>> ListKeyInRevenueAsync(SaleRevenueFilterOptions filterOptions);
    Task<ListResult<object>> GetListAsync(EventFilterOptions filterOptions);
    Task<TResult> DeleteAsync(int id);
    Task<TResult> UpdateAsync(EventUpdateArgs args);
    Task<TResult> CreateAsync(EventCreateArgs args);
    Task<TResult<object>> DetailAsync(Guid id);
    Task<ListResult<object>> ListAsync(EventFilterOptions filterOptions);
    Task<object> OptionsAsync();
    Task<object?> TableOptionsAsync(AllTableFilterOptions filterOptions);
    Task<TResult> CloseDealAsync(CloseDealArgs args);
    Task<List<SUReportResult>> SuReportAsync(SUFilterOptions filterOptions);
    Task<object> KeyInOptionsAsync(KeyInSelectOptions selectOptions);
    Task<object> ToOptionsAsync(SelectOptions selectOptions);
}

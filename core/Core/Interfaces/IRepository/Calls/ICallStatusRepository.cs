using Waffle.Core.Services.Calls.Models;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Calls;

public interface ICallStatusRepository : IAsyncRepository<CallStatus>
{
    Task<bool> IsExistAsync(string code);
    Task<bool> IsUsedAsync(int id);
    Task<ListResult<object>> ListAsync(CallStatusFilterOptions filterOptions);
    Task<object> OptionsAsync(SelectOptions options);
}

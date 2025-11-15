using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository;

public interface ICallCenterRepository : IAsyncRepository<CallCenter>
{
    Task<object> GetOptionsAsync(SelectOptions selectOptions);
}

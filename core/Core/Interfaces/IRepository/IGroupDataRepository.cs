using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository;

public interface IGroupDataRepository : IAsyncRepository<GroupData>
{
    Task<object> GetOptionsAsync(SelectOptions selectOptions);
}

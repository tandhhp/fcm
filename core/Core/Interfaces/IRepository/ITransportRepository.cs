using Waffle.Entities;

namespace Waffle.Core.Interfaces.IRepository;

public interface ITransportRepository : IAsyncRepository<Transport>
{
    Task<object?> GetTransportOptionsAsync();
}

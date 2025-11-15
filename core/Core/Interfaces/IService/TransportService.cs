using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;

namespace Waffle.Core.Services;

public class TransportService(ITransportRepository _transportRepository) : ITransportService
{
    public Task<object?> GetTransportOptionsAsync() => _transportRepository.GetTransportOptionsAsync();
}


namespace Waffle.Core.Interfaces.IService;

public interface ITransportService
{
    Task<object?> GetTransportOptionsAsync();
}

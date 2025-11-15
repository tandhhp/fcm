using Waffle.Entities;

namespace Waffle.Core.Interfaces.IRepository;

public interface IFileRepository : IAsyncRepository<FileContent>
{
    Task<decimal> GetTotalSizeAsync();
}

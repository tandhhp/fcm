using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Data;
using Waffle.Entities;

namespace Waffle.Infrastructure.Repositories;

public class FileRepository : EfRepository<FileContent>, IFileRepository
{
    public FileRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<decimal> GetTotalSizeAsync() => await _context.FileContents.SumAsync(x => x.Size);
}

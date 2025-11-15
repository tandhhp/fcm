using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Data;
using Waffle.Entities;

namespace Waffle.Infrastructure.Repositories;

public class TransportRepository(ApplicationDbContext context) : EfRepository<Transport>(context), ITransportRepository
{
    public async Task<object?> GetTransportOptionsAsync() => await _context.Transports.Select(x => new
    {
        Label = x.Name,
        Value = x.Id
    }).ToListAsync();
}

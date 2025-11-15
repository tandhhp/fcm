using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository.Events;
using Waffle.Core.Services.Rooms.Models;
using Waffle.Core.Services.Tables.Models;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Events;

public class RoomRepository(ApplicationDbContext context) : EfRepository<Room>(context), IRoomRepository
{
    public Task<ListResult<object>> GetListAsync(RoomFilterOptions filterOptions)
    {
        var query = from r in _context.Rooms
                    join d in _context.Branches on r.BranchId equals d.Id
                    select new
                    {
                        r.Id,
                        r.Name,
                        r.BranchId,
                        DistrictName = d.Name,
                        TableCount = _context.Tables.Count(t => t.RoomId == r.Id)
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        if (filterOptions.BranchId.HasValue)
        {
            query = query.Where(x => x.BranchId == filterOptions.BranchId);
        }
        query = query.OrderBy(x => x.Name);
        return ListResult<object>.Success(query, filterOptions);
    }

    public Task<ListResult<object>> GetTablesAsync(TableFilterOptions filterOptions)
    {
        var query = from t in _context.Tables
                    join r in _context.Rooms on t.RoomId equals r.Id
                    join d in _context.Branches on r.BranchId equals d.Id
                    select new
                    {
                        t.Id,
                        t.Name,
                        t.RoomId,
                        RoomName = r.Name,
                        r.BranchId,
                        DistrictName = d.Name
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        if (filterOptions.RoomId.HasValue)
        {
            query = query.Where(x => x.RoomId == filterOptions.RoomId);
        }
        if (filterOptions.BranchId.HasValue)
        {
            query = query.Where(x => x.BranchId == filterOptions.BranchId);
        }
        query = query.OrderBy(x => x.Name);
        return ListResult<object>.Success(query, filterOptions);
    }
}

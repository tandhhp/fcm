using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository.Events;
using Waffle.Core.Services.Tables.Filters;
using Waffle.Core.Services.Tables.ListResults;
using Waffle.Core.Services.Tables.Models;
using Waffle.Data;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Events;

public class TableRepository(ApplicationDbContext context) : EfRepository<Table>(context), ITableRepository
{
    public async Task<TResult<List<AllRoolListItem>>> GetAllTablesAsync(AllTableFilterOptions filterOptions)
    {
        var result = new List<AllRoolListItem>();
        var rooms = await _context.Rooms.AsNoTracking().ToListAsync();
        var tables = await _context.Tables.OrderBy(x => x.SortOrder).AsNoTracking().ToListAsync();
        var feedbacks = await (from l in _context.Leads
                         join f in _context.LeadFeedbacks on l.Id equals f.LeadId
                         where l.EventId == filterOptions.EventId && l.EventDate.Date == filterOptions.EventDate.Date
                         select f.TableId).ToListAsync();
        foreach (var room in rooms)
        {
            var allRoomListItem = new AllRoolListItem
            {
                RoomId = room.Id,
                RoomName = room.Name
            };
            var allTableListItem = new List<AllTableListItem>();
            var roomTables = tables.Where(x => x.RoomId == room.Id);
            foreach (var table in roomTables)
            {
                allTableListItem.Add(new AllTableListItem
                {
                    TableId = table.Id,
                    TableName = table.Name,
                    SortOrder = table.SortOrder,
                    Disabled = feedbacks.Any(x => x == table.Id)
                });
            }
            allRoomListItem.Tables = allTableListItem;
            result.Add(allRoomListItem);
        }
        return TResult<List<AllRoolListItem>>.Ok(result);
    }

    public async Task<object> GetOptionsAsync(SelectOptions selectOptions)
    {
        var query = from t in _context.Tables
                    select new
                    {
                        t.Id,
                        t.Name
                    };
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        return await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync();
    }

    public async Task<ListResult<object>> ListAsync(TableFilterOptions filterOptions)
    {
        var query = from a in _context.Tables
                    join b in _context.Rooms on a.RoomId equals b.Id
                    select new
                    {
                        a.Id,
                        a.Name,
                        a.SortOrder,
                        a.CreatedDate,
                        RoomName = b.Name,
                        a.RoomId,
                        b.BranchId
                    };
        if (filterOptions.RoomId.HasValue)
        {
            query = query.Where(x => x.RoomId == filterOptions.RoomId);
        }
        if (filterOptions.BranchId != null)
        {
            query = query.Where(x => x.BranchId == filterOptions.BranchId);
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<object>.Success(query, filterOptions);
    }
}

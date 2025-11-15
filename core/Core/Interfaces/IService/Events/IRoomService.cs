using Waffle.Core.Services.Rooms.Models;
using Waffle.Core.Services.Tables.Models;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService.Events;

public interface IRoomService
{
    Task<TResult> CreateAsync(RoomCreateArgs args);
    Task<TResult> DeleteAsync(int id);
    Task<Room?> FindAsync(int id);
    Task<ListResult<object>> GetListAsync(RoomFilterOptions filterOptions);
    Task<ListResult<object>> GetTablesAsync(TableFilterOptions filterOptions);
    Task<TResult> UpdateAsync(RoomUpdateArgs args);
}

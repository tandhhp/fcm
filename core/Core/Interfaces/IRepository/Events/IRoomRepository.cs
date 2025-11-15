using Waffle.Core.Services.Rooms.Models;
using Waffle.Core.Services.Tables.Models;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Events;

public interface IRoomRepository : IAsyncRepository<Room>
{
    Task<ListResult<object>> GetListAsync(RoomFilterOptions filterOptions);
    Task<ListResult<object>> GetTablesAsync(TableFilterOptions filterOptions);
}

using Waffle.Core.Interfaces.IRepository.Events;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Interfaces.IService.Events;
using Waffle.Core.Services.Tables.Filters;
using Waffle.Core.Services.Tables.ListResults;
using Waffle.Core.Services.Tables.Models;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Services.Tables;

public class TableService(ITableRepository _tableRepository, IHCAService _hcaService, IRoomService _roomService) : ITableService
{
    public async Task<TResult> CreateAsync(TableCreateArgs args)
    {
        var room = await _roomService.FindAsync(args.RoomId);
        if (room is null) return TResult.Failed("Room not found");
        await _tableRepository.AddAsync(new Table
        {
            Name = args.Name,
            CreatedBy = _hcaService.GetUserId(),
            CreatedDate = DateTime.Now,
            RoomId = args.RoomId,
            SortOrder = args.SortOrder
        });
        return TResult.Success;
    }

    public async Task<TResult> DeleteAsync(int id)
    {
        var data = await _tableRepository.FindAsync(id);
        if (data is null) return TResult.Failed("Table not found");
        await _tableRepository.DeleteAsync(data);
        return TResult.Success;
    }

    public Task<TResult<List<AllRoolListItem>>> GetAllTablesAsync(AllTableFilterOptions filterOptions) => _tableRepository.GetAllTablesAsync(filterOptions);

    public Task<object> GetOptionsAsync(SelectOptions selectOptions) => _tableRepository.GetOptionsAsync(selectOptions);

    public Task<ListResult<object>> ListAsync(TableFilterOptions filterOptions) => _tableRepository.ListAsync(filterOptions);

    public async Task<TResult> UpdateAsync(TableUpdateArgs args)
    {
        var data = await _tableRepository.FindAsync(args.Id);
        if (data is null) return TResult.Failed("Table not found");
        var room = await _roomService.FindAsync(args.RoomId);
        if (room is null) return TResult.Failed("Room not found");
        data.Name = args.Name;
        data.ModifiedBy = _hcaService.GetUserId();
        data.ModifiedDate = DateTime.Now;
        data.SortOrder = args.SortOrder;
        data.RoomId = args.RoomId;
        await _tableRepository.UpdateAsync(data);
        return TResult.Success;
    }
}

using Waffle.Core.Interfaces;
using Waffle.Core.Interfaces.IRepository.Events;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Interfaces.IService.Events;
using Waffle.Core.Services.Rooms.Models;
using Waffle.Core.Services.Tables.Models;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Services.Rooms;

public class RoomService(IRoomRepository _roomRepository, IDistrictService _districtService, IHCAService _hcaService, IBranchService _branchService) : IRoomService
{
    public async Task<TResult> CreateAsync(RoomCreateArgs args)
    {
        var branch = await _branchService.FindAsync(args.BranchId);
        if (branch is null) return TResult.Failed("Quận/Huyện không tồn tại trong hệ thống");
        var room = new Room
        {
            Name = args.Name,
            BranchId = args.BranchId,
            CreatedDate = DateTime.Now,
            CreatedBy = _hcaService.GetUserId()
        };
        await _roomRepository.AddAsync(room);
        return TResult.Success;
    }

    public async Task<TResult> DeleteAsync(int id)
    {
        var data = await FindAsync(id);
        if (data is null) return TResult.Failed("Phòng không tồn tại trong hệ thống");
        await _roomRepository.DeleteAsync(data);
        return TResult.Success;
    }

    public Task<Room?> FindAsync(int id) => _roomRepository.FindAsync(id);

    public Task<ListResult<object>> GetListAsync(RoomFilterOptions filterOptions) => _roomRepository.GetListAsync(filterOptions);

    public Task<ListResult<object>> GetTablesAsync(TableFilterOptions filterOptions) => _roomRepository.GetTablesAsync(filterOptions);

    public async Task<TResult> UpdateAsync(RoomUpdateArgs args)
    {
        var data = await FindAsync(args.Id);
        if (data is null) return TResult.Failed("Phòng không tồn tại trong hệ thống");
        var district = await _districtService.FindAsync(args.BranchId);
        if (district is null) return TResult.Failed("Quận/Huyện không tồn tại trong hệ thống");
        data.Name = args.Name;
        data.BranchId = args.BranchId;
        data.ModifiedDate = DateTime.Now;
        data.ModifiedBy = _hcaService.GetUserId();
        await _roomRepository.UpdateAsync(data);
        return TResult.Success;
    }
}

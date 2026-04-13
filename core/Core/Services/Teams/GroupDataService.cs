using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Teams.Interfaces;
using Waffle.Core.Services.Teams.Models;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Services.Teams;

public class GroupDataService(IGroupDataRepository _groupDataRepository, ILogService _logService) : IGroupDataService
{
    public async Task<TResult> CreateAsync(CreateGroupDataArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.Name)) return TResult.Failed("Tên nhóm dữ liệu không được để trống!");
        if (await _groupDataRepository.ExistsAsync(args.Name)) return TResult.Failed("Tên nhóm dữ liệu đã tồn tại!");
        await _groupDataRepository.AddAsync(new GroupData
        {
            Name = args.Name.Trim()
        });
        await _logService.AddAsync($"Đã tạo Group Data: {args.Name}");
        return TResult.Success;
    }

    public async Task<TResult> DeleteAsync(int id)
    {
        var groupData = await _groupDataRepository.FindAsync(id);
        if (groupData is null) return TResult.Failed("Không tìm thấy nhóm dữ liệu!");
        if (await _groupDataRepository.HasTeamAsync(id)) return TResult.Failed("Không thể xóa vì đã có team đang sử dụng nhóm dữ liệu này!");
        await _groupDataRepository.DeleteAsync(groupData);
        await _logService.AddAsync($"Đã xóa Group Data: {groupData.Name}");
        return TResult.Success;
    }

    public async Task<TResult<object>> DetailAsync(int id)
    {
        var groupData = await _groupDataRepository.FindAsync(id);
        if (groupData is null) return TResult<object>.Failed("Không tìm thấy nhóm dữ liệu!");
        return TResult<object>.Ok(new
        {
            groupData.Id,
            groupData.Name
        });
    }

    public Task<GroupData?> FindAsync(int id) => _groupDataRepository.FindAsync(id);

    public Task<ListResult<object>> ListAsync(GroupDataFilterOptions filterOptions) => _groupDataRepository.ListAsync(filterOptions);

    public Task<object> GetOptionsAsync(SelectOptions selectOptions) => _groupDataRepository.GetOptionsAsync(selectOptions);

    public async Task<TResult> UpdateAsync(UpdateGroupDataArgs args)
    {
        var groupData = await _groupDataRepository.FindAsync(args.Id);
        if (groupData is null) return TResult.Failed("Không tìm thấy nhóm dữ liệu!");
        if (string.IsNullOrWhiteSpace(args.Name)) return TResult.Failed("Tên nhóm dữ liệu không được để trống!");
        if (await _groupDataRepository.ExistsAsync(args.Name, args.Id)) return TResult.Failed("Tên nhóm dữ liệu đã tồn tại!");
        groupData.Name = args.Name.Trim();
        await _groupDataRepository.UpdateAsync(groupData);
        await _logService.AddAsync($"Đã cập nhật Group Data: {groupData.Name} (ID: {groupData.Id})");
        return TResult.Success;
    }
}

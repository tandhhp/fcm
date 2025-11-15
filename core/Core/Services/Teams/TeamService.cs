using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Teams.Args;
using Waffle.Core.Services.Teams.Models;
using Waffle.Entities;
using Waffle.Entities.Users;
using Waffle.Models;

namespace Waffle.Core.Services.Teams;

public class TeamService(ITeamRepository _teamRepository, IDepartmentService _departmentService, UserManager<ApplicationUser> _userManager) : ITeamService
{
    public async Task<TResult> AddUserAsync(TeamUserAddArgs args)
    {
        var team = await _teamRepository.FindAsync(args.TeamId);
        if (team is null) return TResult.Failed("Không tìm thấy nhóm!");
        var user = await _userManager.FindByIdAsync(args.UserId.ToString());
        if (user is null) return TResult.Failed("Không tìm thấy người dùng!");
        user.TeamId = args.TeamId;
        user.ManagerId = team.LeaderId;
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded ? TResult.Success : TResult.Failed(result.Errors.FirstOrDefault()?.Description ?? "Có lỗi xảy ra!");
    }

    public async Task<TResult> CreateAsync(CreateTeamArgs args)
    {
        var department = await _departmentService.FindAsync(args.DepartmentId);
        if (department is null) return TResult.Failed("Không tìm thấy phòng ban!");
        if (string.IsNullOrWhiteSpace(args.Name)) return TResult.Failed("Tên nhóm không được để trống!");
        if (await _teamRepository.ExistsAsync(args.Name, args.DepartmentId)) return TResult.Failed("Tên nhóm đã tồn tại trong phòng ban này!");
        var team = new Team
        {
            Name = args.Name,
            DepartmentId = args.DepartmentId,
            LeaderId = args.LeaderId,
            CallCenterId = args.CallCenterId,
            GroupDataId = args.GroupDataId
        };
        await _teamRepository.AddAsync(team);
        return TResult.Success;
    }

    public async Task<TResult> DeleteAsync(int id)
    {
        var team = await _teamRepository.FindAsync(id);
        if (team is null) return TResult.Failed("Không tìm thấy nhóm!");
        if (await _userManager.Users.AnyAsync(x => x.TeamId == team.Id))
        {
            return TResult.Failed("Không thể xóa nhóm vì có người dùng đang thuộc nhóm này!");
        }
        await _teamRepository.DeleteAsync(team);
        return TResult.Success;
    }

    public async Task<TResult<object>> DetailAsync(int id)
    {
        var team = await _teamRepository.FindAsync(id);
        if (team is null) return TResult<object>.Failed("Không tìm thấy nhóm!");
        var leader = team.LeaderId.HasValue ? await _userManager.FindByIdAsync(team.LeaderId.Value.ToString()) : null;
        return TResult<object>.Ok(new
        {
            team.Id,
            team.Name,
            team.DepartmentId,
            team.LeaderId,
            team.CallCenterId,
            team.GroupDataId,
            LeaderName = leader?.Name
        });
    }

    public Task<Team?> FindAsync(int id) => _teamRepository.FindAsync(id);

    public Task<ListResult<object>> ListAsync(TeamFilterOptions filterOptions) => _teamRepository.ListAsync(filterOptions);

    public Task<object?> OptionsAsync(TeamSelectOptions selectOptions) => _teamRepository.OptionsAsync(selectOptions);

    public async Task<TResult> RemoveUserAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null) return TResult.Failed("Không tìm thấy người dùng!");
        user.TeamId = null;
        user.ManagerId = null;
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded ? TResult.Success : TResult.Failed(result.Errors.FirstOrDefault()?.Description ?? "Có lỗi xảy ra!");
    }

    public async Task<TResult> UpdateAsync(UpdateTeamArgs args)
    {
        var team = await _teamRepository.FindAsync(args.Id);
        if (team is null) return TResult.Failed("Không tìm thấy nhóm!");
        var department = await _departmentService.FindAsync(args.DepartmentId);
        if (department is null) return TResult.Failed("Không tìm thấy phòng ban!");
        if (args.LeaderId.HasValue)
        {
            var leader = await _userManager.FindByIdAsync(args.LeaderId.Value.ToString());
            if (leader is null) return TResult.Failed("Không tìm thấy trưởng nhóm!");
            team.LeaderId = args.LeaderId;
        }
        team.Name = args.Name;
        team.DepartmentId = args.DepartmentId;
        team.CallCenterId = args.CallCenterId;
        team.GroupDataId = args.GroupDataId;
        await _teamRepository.UpdateAsync(team);
        return TResult.Success;
    }

    public Task<ListResult<object>> UsersAsync(UserTeamFilterOptions filterOptions) => _teamRepository.UsersAsync(filterOptions);
}

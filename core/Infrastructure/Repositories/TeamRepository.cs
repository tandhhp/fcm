using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Services.Teams.Models;
using Waffle.Data;
using Waffle.Entities.Users;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories;

public class TeamRepository(ApplicationDbContext context) : EfRepository<Team>(context), ITeamRepository
{
    public async Task<bool> ExistsAsync(string name, int departmentId) => await _context.Teams.AnyAsync(t => t.Name == name && t.DepartmentId == departmentId);

    public async Task<ListResult<object>> ListAsync(TeamFilterOptions filterOptions)
    {
        var query = from t in _context.Teams
                    join d in _context.Departments on t.DepartmentId equals d.Id
                    join l in _context.Users on t.LeaderId equals l.Id into leaders
                    from leader in leaders.DefaultIfEmpty()
                    join cc in _context.CallCenters on t.CallCenterId equals cc.Id into callCenters
                    from callCenter in callCenters.DefaultIfEmpty()
                    join gd in _context.GroupDatas on t.GroupDataId equals gd.Id into groupDatas
                    from groupData in groupDatas.DefaultIfEmpty()
                    select new
                    {
                        t.Id,
                        t.Name,
                        t.DepartmentId,
                        DepartmentName = d.Name,
                        UserCount = _context.Users.Count(u => u.TeamId == t.Id),
                        LeaderName = leader.Name,
                        CallCenterName = callCenter.Name,
                        GroupDataName = groupData.Name,
                        t.GroupDataId,
                        t.CallCenterId,
                        t.LeaderId
                    };
        if (filterOptions.DepartmentId != null)
        {
            query = query.Where(t => t.DepartmentId == filterOptions.DepartmentId);
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(t => t.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        query = query.OrderBy(t => t.Name);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<object?> OptionsAsync(TeamSelectOptions selectOptions)
    {
        var query = from t in _context.Teams
                    select new
                    {
                        t.Id,
                        t.Name,
                        t.DepartmentId
                    };
        if (selectOptions.DepartmentId != null)
        {
            query = query.Where(t => t.DepartmentId == selectOptions.DepartmentId);
        }
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(t => t.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        query = query.OrderBy(t => t.Name);
        return await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync();
    }

    public async Task<ListResult<object>> UsersAsync(UserTeamFilterOptions filterOptions)
    {
        var query = from u in _context.Users
                    where u.TeamId == filterOptions.TeamId
                    select new
                    {
                        u.Id,
                        u.UserName,
                        u.Email,
                        u.TeamId,
                        u.Name,
                        u.Gender,
                        u.PhoneNumber,
                        u.Avatar
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(u => u.Name.Contains(filterOptions.Name, StringComparison.CurrentCultureIgnoreCase));
        }
        return await ListResult<object>.Success(query, filterOptions);
    }
}

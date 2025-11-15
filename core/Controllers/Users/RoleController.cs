using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waffle.Core.Constants;
using Waffle.Core.Services.Roles.Filters;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Extensions;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers.Users;

public class RoleController(RoleManager<ApplicationRole> _roleManager, ApplicationDbContext _context, UserManager<ApplicationUser> _userManager) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] BasicFilterOptions filterOptions)
    {
        var user = await _userManager.FindByIdAsync(User.GetClaimId());
        if (user == null) return Unauthorized();
        var isAdmin = User.IsInRole(RoleName.Admin);
        var query = from a in _context.Roles
                    where a.Name != RoleName.CardHolder && a.Name != RoleName.Admin
                    select new
                    {
                        a.Id,
                        a.Name,
                        a.DisplayName,
                        total = (from a1 in _context.UserRoles
                                 join b1 in _context.Users on a1.UserId equals b1.Id
                                 where b1.Status == UserStatus.Working && a1.RoleId == a.Id && (b1.BranchId == user.BranchId || isAdmin)
                                 select a1.UserId).Count(),
                        leave = (from a1 in _context.UserRoles
                                 join b1 in _context.Users on a1.UserId equals b1.Id
                                 where b1.Status == UserStatus.Leave && a1.RoleId == a.Id && (b1.BranchId == user.BranchId || isAdmin)
                                 select a1.UserId).Count(),
                        a.Description
                    };
        if (User.IsInRole(RoleName.Dos))
        {
            query = query.Where(x => x.Name == RoleName.Sales || x.Name == RoleName.SalesManager);
        }
        if (User.IsInRole(RoleName.SalesManager))
        {
            query = query.Where(x => x.Name == RoleName.Sales);
        }

        query = query.OrderBy(x => x.Name);

        return Ok(await ListResult<object>.Success(query, filterOptions));
    }

    [HttpGet("user-options-in-role/{name}")]
    public async Task<IActionResult> GetUserOptionsInRoleAsync([FromRoute] string name)
    {
        var users = await _userManager.GetUsersInRoleAsync(name);
        var user = await _userManager.FindByIdAsync(User.GetClaimId());
        if (user == null) return Unauthorized();
        users = users.Where(x => x.Status == UserStatus.Working).Where(x => x.BranchId == user.BranchId).ToList();
        return Ok(users.Select(x => new
        {
            label = $"{x.Name} - {x.UserName}",
            value = x.Id
        }));
    }

    [HttpPost("delete/{name}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string name)
    {
        var role = await _roleManager.FindByNameAsync(name);
        if (role == null) return NotFound($"Role {name} not found.");
        return Ok(await _roleManager.DeleteAsync(role));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ApplicationRole role) => Ok(await _roleManager.CreateAsync(role));

    [HttpGet("sales-manager-options")]
    public async Task<IActionResult> GetSalesManagerOptionsAsync([FromQuery] SelectOptions selectOptions)
    {
        var query = from a in _context.Users
                    join b in _context.UserRoles on a.Id equals b.UserId
                    join c in _context.Roles on b.RoleId equals c.Id
                    where c.Name == RoleName.SalesManager && a.Status == UserStatus.Working
                    select new
                    {
                        a.Name,
                        a.Id
                    };
        if (!string.IsNullOrEmpty(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        return Ok(await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync());
    }

    [HttpGet("options")]
    public async Task<IActionResult> GetOptionsAsync()
    {
        var roles = await _roleManager.Roles.Where(x => x.Name != RoleName.CardHolder && x.Name != RoleName.Admin).ToListAsync();
        return Ok(roles.Select(x => new
        {
            label = x.DisplayName,
            value = x.Id
        }));
    }

    [HttpGet("dot-options")]
    public async Task<IActionResult> GetDotOptionsAsync([FromQuery] SelectOptions selectOptions)
    {
        var query = from a in _context.Users
                    join b in _context.UserRoles on a.Id equals b.UserId
                    join c in _context.Roles on b.RoleId equals c.Id
                    where c.Name == RoleName.Dot && a.Status == UserStatus.Working
                    select new
                    {
                        a.Name,
                        a.Id
                    };
        if (!string.IsNullOrEmpty(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        return Ok(await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync());
    }

    [HttpGet("telesales-manager-options")]
    public async Task<IActionResult> GetTelesalesManagerOptionsAsync([FromQuery] TelesalesManagerSelectOptions selectOptions)
    {
        var query = from a in _context.Users
                    join b in _context.UserRoles on a.Id equals b.UserId
                    join c in _context.Roles on b.RoleId equals c.Id
                    where c.Name == RoleName.TelesaleManager && a.Status == UserStatus.Working
                    select new
                    {
                        a.Name,
                        a.Id,
                        a.DotId
                    };
        if (selectOptions.DotId.HasValue)
        {
            query = query.Where(x => x.DotId == selectOptions.DotId);
        }
        return Ok(await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync());
    }

    [HttpGet("telesales-options")]
    public async Task<IActionResult> GetTelesalesOptionsAsync([FromQuery] TelesalesSelectOptions selectOptions)
    {
        var query = from a in _context.Users
                    join b in _context.UserRoles on a.Id equals b.UserId
                    join c in _context.Roles on b.RoleId equals c.Id
                    where c.Name == RoleName.Telesale && a.Status == UserStatus.Working
                    select new
                    {
                        a.Name,
                        a.Id,
                        a.TmId,
                        a.DotId
                    };
        if (selectOptions.TelesalesManagerId.HasValue)
        {
            query = query.Where(x => x.TmId == selectOptions.TelesalesManagerId);
        }
        if (selectOptions.DotId.HasValue)
        {
            query = query.Where(x => x.DotId == selectOptions.DotId);
        }
        if (!string.IsNullOrEmpty(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        return Ok(await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync());
    }

    [HttpGet("init")]
    public async Task<IActionResult> InitRolesAsync()
    {
        var roles = new List<ApplicationRole>
        {
            new() { Name = RoleName.Admin, DisplayName = "Ban lãnh đạo", Description = "Ban lãnh đạo" },
            new() { Name = RoleName.CardHolder, DisplayName = "Chủ thẻ", Description = "Chủ thẻ" },
            new() { Name = RoleName.Accountant, DisplayName = "Kế toán", Description = "Kế toán" },
            new() { Name = RoleName.Sales, DisplayName = "Nhân viên kinh doanh", Description = "Nhân viên kinh doanh" },
            new() { Name = RoleName.SalesManager, DisplayName = "Quản lý kinh doanh", Description = "Quản lý kinh doanh" },
            new() { Name = RoleName.Dos, DisplayName = "Giám đốc quan hệ khách hàng", Description = "Giám đốc quan hệ khách hàng" },
            new() { Name = RoleName.CxTP, DisplayName = "Trưởng phòng chăm sóc khách hàng", Description = "Trưởng phòng chăm sóc khách hàng" },
            new() { Name = RoleName.Cx, DisplayName = "Nhân viên chăm sóc khách hàng", Description = "Nhân viên chăm sóc khách hàng" },
            new() { Name = RoleName.Hr, DisplayName = "Nhân sự", Description = "Nhân sự" },
            new() { Name = RoleName.Event, DisplayName = "Sự kiện", Description = "Sự kiện" },
            new() { Name = RoleName.Telesale, DisplayName = "Telesale", Description = "Telesale" },
            new() { Name = RoleName.TelesaleManager, DisplayName = "Trưởng nhóm Telesale", Description = "Trưởng nhóm Telesale" },
            new() { Name = RoleName.Dot, DisplayName = "Giám đốc Telesale", Description = "Giám đốc Telesale" },
            new() { Name = RoleName.AdminData, DisplayName = "Quản trị dữ liệu", Description = "Quản trị dữ liệu" }
        };
        foreach (var role in roles)
        {
            if (string.IsNullOrEmpty(role.Name)) continue;
            var existingRole = await _roleManager.FindByNameAsync(role.Name);
            if (existingRole == null)
            {
                await _roleManager.CreateAsync(role);
            }
        }
        return Ok("Roles initialized successfully.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] ApplicationRole role)
    {
        var existingRole = await _roleManager.FindByIdAsync(role.Id.ToString());
        if (existingRole == null) return NotFound($"Role with ID {role.Id} not found.");
        existingRole.DisplayName = role.DisplayName;
        existingRole.Description = role.Description;
        var result = await _roleManager.UpdateAsync(existingRole);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return BadRequest(errors);
        }
        return Ok(TResult.Success);
    }

    [HttpGet("manager-options")]
    public async Task<IActionResult> GetManagerOptionsAsync([FromQuery] SelectOptions selectOptions)
    {
        var query = from a in _context.Users
                    join b in _context.UserRoles on a.Id equals b.UserId
                    join c in _context.Roles on b.RoleId equals c.Id
                    where (c.Name == RoleName.SalesManager || c.Name == RoleName.TelesaleManager) && a.Status == UserStatus.Working
                    select new
                    {
                        a.Name,
                        a.Id
                    };
        if (!string.IsNullOrEmpty(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        return Ok(await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync());
    }

    [HttpGet("key-in-options")]
    public async Task<IActionResult> GetKeyInOptionsAsync([FromQuery] KeyInSelectOptions selectOptions)
    {
        var roleName = RoleName.Sales;
        if (selectOptions.TeamKeyInId.HasValue)
        {
            var teamKeyIn = await _context.Users.FindAsync(selectOptions.TeamKeyInId.Value);
            if (teamKeyIn is null) return BadRequest("Không tìm thấy trưởng nhóm!");
            if (await _userManager.IsInRoleAsync(teamKeyIn, RoleName.TelesaleManager))
            {
                roleName = RoleName.Telesale;
            }
        }
        var query = from a in _context.Users
                    join b in _context.UserRoles on a.Id equals b.UserId
                    join c in _context.Roles on b.RoleId equals c.Id
                    where c.Name == roleName && a.Status == UserStatus.Working
                    select new
                    {
                        a.Name,
                        a.Id,
                        a.SmId,
                        a.TmId
                    };
        if (selectOptions.TeamKeyInId.HasValue)
        {
            if (roleName == RoleName.Sales)
            {
                query = query.Where(x => x.SmId == selectOptions.TeamKeyInId);
            }
            if (roleName == RoleName.Telesale)
            {
                query = query.Where(x => x.TmId == selectOptions.TeamKeyInId);
            }
        }
        if (!string.IsNullOrEmpty(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        return Ok(await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync());
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Data;
using Waffle.Core.Constants;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Users.Args;
using Waffle.Core.Services.Users.Filters;
using Waffle.Core.Services.Users.Models;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Services;

public class UserService(UserManager<ApplicationUser> _userManager, RoleManager<ApplicationRole> _roleManager, ApplicationDbContext _context, IHCAService _hcaService) : IUserService
{
    private async Task<ApplicationUser?> FindAsync(Guid id) => await _context.Users.FindAsync(id);

    public async Task<TResult> AddToRoleAsync(AddToRoleModel model)
    {
        var user = await FindAsync(model.Id);
        if (user is null) return TResult.Failed("User not found!");
        if (!await _roleManager.RoleExistsAsync(model.RoleName)) return TResult.Failed("Role not found!");
        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
        if (!result.Succeeded) return TResult.Failed(string.Join(", ", result.Errors.Select(x => x.Description)));
        return TResult.Success;
    }

    public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel args)
    {
        var user = await FindAsync(args.Id);
        if (user is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "UserNotFound",
                Description = "User not found!"
            });
        }
        return await _userManager.ChangePasswordAsync(user, args.CurrentPassword, args.NewPassword);
    }

    public async Task<IdentityResult> CreateAsync(CreateUserModel model)
    {
        var user = new ApplicationUser
        {
            Email = model.Email,
            UserName = model.ContractCode,
            PhoneNumber = model.PhoneNumber,
            Address = model.Address,
            Avatar = model.Avatar,
            Gender = model.Gender,
            DateOfBirth = model.DateOfBirth,
            Loyalty = 0,
            Name = model.Name,
            CreatedDate = DateTime.Now,
            ContractCode = model.ContractCode,
            IdentityNumber = model.IdentityNumber,
            ContractDate = model.ContractDate,
            BranchId = model.BranchId
        };
        model.Password = $"nuras@{user.UserName}";
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            var role = await _roleManager.FindByNameAsync(RoleName.CardHolder);
            if (role is null)
            {
                await _roleManager.CreateAsync(new ApplicationRole
                {
                    DisplayName = "Chủ thẻ",
                    Name = RoleName.CardHolder
                });
            }
            await _userManager.AddToRoleAsync(user, RoleName.CardHolder);
        }
        return result;
    }

    public async Task<dynamic> GetUsersInRoleAsync(string roleName, UserFilterOptions filterOptions)
    {
        var query = from a in _context.Users
                    join b in _context.UserRoles on a.Id equals b.UserId
                    join c in _context.Roles on b.RoleId equals c.Id
                    where c.Name == roleName
                    select new
                    {
                        a.Id,
                        a.UserName,
                        a.Name,
                        a.PhoneNumber,
                        a.Email,
                        a.Status,
                        a.BranchId,
                        a.SmId,
                        a.TmId,
                        a.DateOfBirth,
                        a.Position,
                        a.Address,
                        a.ContractDate,
                        TeamName = a.ManagerId != null ? _context.Users.First(t => t.Id == a.ManagerId).Name : null,
                        a.ManagerId,
                        a.DosId,
                        a.DotId,
                        a.Gender,
                        a.Avatar,
                        a.ContractCode,
                        a.CreatedDate,
                        a.DistrictId,
                        a.IdentityNumber,
                        a.LineCode,
                    };
        if (filterOptions.Status != null)
        {
            query = query.Where(x => x.Status == filterOptions.Status);
        }
        if (filterOptions.SmId != null)
        {
            query = query.Where(x => x.SmId == filterOptions.SmId);
        }
        if (filterOptions.TmId != null)
        {
            query = query.Where(x => x.TmId == filterOptions.TmId);
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.UserName))
        {
            query = query.Where(x => x.UserName != null && x.UserName.ToLower().Contains(filterOptions.UserName.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.ToLower().Contains(filterOptions.PhoneNumber));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Email))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Contains(filterOptions.Email.ToLower()));
        }
        return await ListResult<object>.Success(query, filterOptions);
    }
    public async Task<IdentityResult> RemoveFromRoleAsync(RemoveFromRoleModel args)
    {
        var user = await FindAsync(args.Id);
        if (user is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "User not found!"
            });
        }
        return await _userManager.RemoveFromRoleAsync(user, args.RoleName);
    }

    public async Task<TResult<object>> DetailAsync(Guid id)
    {
        var user = await FindAsync(id);
        if (user is null) return TResult<object>.Failed("Không tìm thấy chủ thẻ!");
        var district = new District();
        if (user.DistrictId != null)
        {
            district = await _context.Districts.FindAsync(user.DistrictId);
        }
        return TResult<object>.Ok(new
        {
            user.Id,
            user.UserName,
            user.Name,
            user.PhoneNumber,
            user.Email,
            user.Address,
            user.BranchId,
            user.Gender,
            user.DateOfBirth,
            user.Avatar,
            user.IdentityNumber,
            user.DistrictId,
            district?.ProvinceId,
            user.CreatedDate
        });
    }

    public async Task<object?> GetTelesalesOptionsAsync(TelesalesSelectOptions selectOptions)
    {
        var query = from a in _context.Users
                    join b in _context.UserRoles on a.Id equals b.UserId
                    join c in _context.Roles on b.RoleId equals c.Id
                    where c.Name == RoleName.Telesales
                    where a.Status == UserStatus.Working
                    select new
                    {
                        a.Id,
                        a.Name,
                        a.TmId,
                        a.TeamId,
                        a.BranchId
                    };
        if (selectOptions.TelesalesManagerId != null)
        {
            query = query.Where(x => x.TmId == selectOptions.TelesalesManagerId);
        }
        if (selectOptions.TeamId.HasValue)
        {
            query = query.Where(x => x.TeamId == selectOptions.TeamId);
        }
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        if (selectOptions.BranchId.HasValue)
        {
            query = query.Where(x => x.BranchId == selectOptions.BranchId);
        }
        return await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync();
    }

    public async Task<TResult> ImportAsync(UserImportArgs args)
    {
        try
        {
            if (!_hcaService.IsUserInAnyRole(RoleName.Hr, RoleName.Admin)) return TResult.Failed("Bạn không có quyền thực hiện hành động này!");
            if (args.File is null || args.File.Length == 0) return TResult.Failed("Chưa chọn file!");
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();

            using var pgk = new ExcelPackage(args.File.OpenReadStream());
            var worksheet = pgk.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;
            var data = await _userManager.Users.Select(x => x.UserName).ToListAsync();

            var salesManagers = await _userManager.GetUsersInRoleAsync(RoleName.SalesManager);
            var telesalesManagers = await _userManager.GetUsersInRoleAsync(RoleName.TelesaleManager);
            var directorsOfSales = await _userManager.GetUsersInRoleAsync(RoleName.DOS);
            var directorsOfTele = await _userManager.GetUsersInRoleAsync(RoleName.Dot);

            for (int i = 2; i <= rowCount; i++)
            {
                var userName = worksheet.Cells[i, 1].Value?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(userName)) continue;

                var roleName = worksheet.Cells[i, 10].Value?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(roleName)) continue;
                if (!roles.Any(x => x.Name != null && x.Name == roleName)) TResult.Failed($"Không tìm thấy quyền {roleName}!");

                if (data.Any(x => x != null && x.Equals(userName, StringComparison.OrdinalIgnoreCase)))
                {
                    var userData = await _userManager.FindByNameAsync(userName);
                    if (userData is null) continue;
                    var rolesOfUser = await _userManager.GetRolesAsync(userData);
                    if (!rolesOfUser.Any())
                    {
                        await _userManager.AddToRoleAsync(userData, roleName);
                        continue;
                    }
                    return TResult.Failed($"Người dùng {userName} đã tồn tại!");
                }

                var name = worksheet.Cells[i, 2].Value?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(name)) continue;
                var gender = worksheet.Cells[i, 3].Value?.ToString()?.Trim();
                var dateOfBirthString = worksheet.Cells[i, 4].Value?.ToString()?.Trim();
                var dateOfBirth = DateTimeHelper.ParseDateOnly(dateOfBirthString);
                var phoneNumber = worksheet.Cells[i, 5].Value?.ToString()?.Trim();
                var email = worksheet.Cells[i, 6].Value?.ToString()?.Trim();
                var lineCode = worksheet.Cells[i, 7].Value?.ToString()?.Trim();
                var workStartDateString = worksheet.Cells[i, 8].Value?.ToString()?.Trim();
                var workStartDate = DateTimeHelper.ParseDateOnly(workStartDateString);
                var position = worksheet.Cells[i, 9].Value?.ToString()?.Trim();
                Guid? salesManagerId = null;
                Guid? directorOfSalesId = null;
                Guid? telesalesManagerId = null;
                Guid? directorOfTeleId = null;
                var salesManagerUserName = worksheet.Cells[i, 11].Value?.ToString()?.Trim();
                if (!string.IsNullOrWhiteSpace(salesManagerUserName))
                {
                    var sm = salesManagers.FirstOrDefault(x => x.UserName == salesManagerUserName);
                    if (sm is null) return TResult.Failed("Không tìm thấy quản lý kinh doanh!");
                    salesManagerId = sm.Id;
                    directorOfSalesId = sm.DosId;
                }
                var directorOfSalesUserName = worksheet.Cells[i, 12].Value?.ToString()?.Trim();
                if (!string.IsNullOrWhiteSpace(directorOfSalesUserName))
                {
                    var dos = directorsOfSales.FirstOrDefault(x => x.UserName == directorOfSalesUserName);
                    if (dos is null) return TResult.Failed($"Không tìm thấy giám đốc quan hệ khách hàng tại dòng {i}!");
                    directorOfSalesId = dos.Id;
                }
                var telesalesManagerUserName = worksheet.Cells[i, 13].Value?.ToString()?.Trim();
                if (!string.IsNullOrWhiteSpace(telesalesManagerUserName))
                {
                    var tm = telesalesManagers.FirstOrDefault(x => x.UserName == telesalesManagerUserName);
                    if (tm is null) return TResult.Failed($"Không tìm thấy Telesales Manager tại dòng {i}!");
                    telesalesManagerId = tm.Id;
                    directorOfTeleId = tm.DotId;
                }
                var directorOfTeleUserName = worksheet.Cells[i, 14].Value?.ToString()?.Trim();
                if (!string.IsNullOrWhiteSpace(directorOfTeleUserName))
                {
                    var dot = directorsOfTele.FirstOrDefault(x => x.UserName == directorOfTeleUserName);
                    if (dot is null) return TResult.Failed($"Không tìm thấy giám đốc telesales tại dòng {i}!");
                    directorOfTeleId = dot.Id;
                }
                var teamName = worksheet.Cells[i, 15].Value?.ToString()?.Trim();
                int? teamId = null;
                if (!string.IsNullOrWhiteSpace(teamName))
                {
                    var team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(x => x.Name == teamName);
                    if (team is null) return TResult.Failed($"Đội nhóm không thấy tại dòng {i}!");
                    teamId = team.Id;
                }
                var password = worksheet.Cells[i, 16].Value?.ToString()?.Trim();
                if (string.IsNullOrEmpty(password)) continue;
                var user  = await _userManager.FindByNameAsync(userName ?? string.Empty);
                if (user is null)
                {
                    user = new ApplicationUser
                    {
                        UserName = userName,
                        Name = name,
                        PhoneNumber = phoneNumber,
                        Email = email,
                        Gender = !"Nam".Equals(gender, StringComparison.OrdinalIgnoreCase),
                        DateOfBirth = dateOfBirth,
                        BranchId = args.BranchId,
                        CreatedDate = DateTime.Now,
                        Status = UserStatus.Working,
                        TeamId = teamId,
                        TmId = telesalesManagerId,
                        SmId = salesManagerId,
                        DosId = directorOfSalesId,
                        DotId = directorOfTeleId,
                        LineCode = lineCode,
                        ContractDate = workStartDate,
                        Position = position
                    };
                    if (RoleName.Sales.Equals(roleName))
                    {
                        user.ManagerId = salesManagerId;
                    }
                    if (RoleName.Telesales.Equals(roleName))
                    {
                        user.ManagerId = telesalesManagerId;
                    }
                    if (RoleName.SalesManager.Equals(roleName))
                    {
                        user.ManagerId = directorOfSalesId;
                    }
                    if (RoleName.TelesaleManager.Equals(roleName))
                    {
                        user.ManagerId = directorOfTeleId;
                    }
                    await _userManager.CreateAsync(user, password);
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            return TResult.Success;
        }
        catch (Exception ex)
        {
            return TResult.Failed(ex.ToString());
        }
    }

    public async Task<TResult<byte[]?>> ExportAsync(UserFilterOptions filterOptions)
    {
        var query = from a in _context.Users
                    join b in _context.UserRoles on a.Id equals b.UserId
                    join c in _context.Roles on b.RoleId equals c.Id
                    select new
                    {
                        a.Id,
                        a.UserName,
                        a.Name,
                        a.DateOfBirth,
                        a.Gender,
                        a.PhoneNumber,
                        a.Email,
                        a.Status,
                        RoleName = c.DisplayName,
                        a.ContractDate,
                        a.LineCode,
                        a.Position
                    };
        var users = await query.ToListAsync();
        using var pgk = new ExcelPackage();
        var worksheet = pgk.Workbook.Worksheets.Add("Users");
        worksheet.Cells[1, 1].Value = "STT";
        worksheet.Cells[1, 2].Value = "Tên đăng nhập";
        worksheet.Cells[1, 3].Value = "Họ và tên";
        worksheet.Cells[1, 4].Value = "Ngày sinh";
        worksheet.Cells[1, 5].Value = "Giới tính";
        worksheet.Cells[1, 6].Value = "Số điện thoại";
        worksheet.Cells[1, 7].Value = "Email";
        worksheet.Cells[1, 8].Value = "Trạng thái";
        worksheet.Cells[1, 9].Value = "Vai trò";
        worksheet.Cells[1, 10].Value = "Ngày vào làm";
        worksheet.Cells[1, 11].Value = "Mã tuyến";
        worksheet.Cells[1, 12].Value = "Chức vụ";

        worksheet.Row(1).Style.Font.Bold = true;

        var row = 2;
        foreach (var user in users)
        {
            worksheet.Cells[row, 1].Value = row - 1;
            worksheet.Cells[row, 2].Value = user.UserName;
            worksheet.Cells[row, 3].Value = user.Name;
            worksheet.Cells[row, 4].Value = user.DateOfBirth?.ToString("dd/MM/yyyy");
            worksheet.Cells[row, 5].Value = user.Gender == true ? "Nữ" : "Nam";
            worksheet.Cells[row, 6].Value = user.PhoneNumber;
            worksheet.Cells[row, 7].Value = user.Email;
            worksheet.Cells[row, 8].Value = user.Status == UserStatus.Working ? "Đang làm việc" : "Đã nghỉ việc";
            worksheet.Cells[row, 9].Value = user.RoleName;
            worksheet.Cells[row, 10].Value = user.ContractDate?.ToString("dd/MM/yyyy");
            worksheet.Cells[row, 11].Value = user.LineCode;
            worksheet.Cells[row, 12].Value = user.Position;
            row++;
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        var cells = worksheet.Cells[worksheet.Dimension.Address];
        cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        return TResult<byte[]?>.Ok(await pgk.GetAsByteArrayAsync());
    }

    public async Task<ListResult<object>> ListDosAsync(FilterOptions filterOptions)
    {
        var query = from a in _context.Users
                    join b in _context.UserRoles on a.Id equals b.UserId
                    join c in _context.Roles on b.RoleId equals c.Id
                    where c.Name == RoleName.DOS && a.Status == UserStatus.Working
                    select new
                    {
                        a.Id,
                        a.Name,
                        a.CreatedDate
                    };
        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<TResult> SetDosAsync(SetDosArgs args)
    {
        var tele = await FindAsync(args.TeleId);
        if (tele is null) return TResult.Failed("Không tìm thấy người dùng!");
        var dos = await FindAsync(args.DosId);
        if (dos is null) return TResult.Failed("Không tìm thấy giám đốc quan hệ khách hàng!");
        if (!await _userManager.IsInRoleAsync(dos, RoleName.DOS)) return TResult.Failed("Người dùng không phải là giám đốc quan hệ khách hàng!");
        tele.DosId = dos.Id;
        await _userManager.UpdateAsync(tele);
        return TResult.Success;
    }

    public async Task<ListResult<object>> ListTelesalesAsync(TelesaleFilterOptions filterOptions)
    {
        var query = from a in _context.Users
                    join b in _context.UserRoles on a.Id equals b.UserId
                    join c in _context.Roles on b.RoleId equals c.Id
                    join d in _context.Branches on a.BranchId equals d.Id into branchGroup
                    from branch in branchGroup.DefaultIfEmpty()
                    join e in _context.Teams on a.TeamId equals e.Id into teamGroup
                    from team in teamGroup.DefaultIfEmpty()
                    where c.Name == RoleName.Telesales
                    select new
                    {
                        a.Id,
                        a.UserName,
                        a.Name,
                        a.Email,
                        a.PhoneNumber,
                        a.Gender,
                        a.DateOfBirth,
                        a.BranchId,
                        BranchName = branch != null ? branch.Name : null,
                        a.TeamId,
                        TeamName = team != null ? team.Name : null,
                        a.LineCode,
                        a.Avatar,
                        a.Status,
                        a.CreatedDate,
                        a.ManagerId
                    };

        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filterOptions.UserName))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.UserName) && x.UserName.ToLower().Contains(filterOptions.UserName.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filterOptions.Email))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Contains(filterOptions.Email.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.Contains(filterOptions.PhoneNumber));
        }

        if (filterOptions.BranchId.HasValue)
        {
            query = query.Where(x => x.BranchId == filterOptions.BranchId.Value);
        }

        if (filterOptions.TeamId.HasValue)
        {
            query = query.Where(x => x.TeamId == filterOptions.TeamId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filterOptions.LineCode))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.LineCode) && x.LineCode.ToLower().Contains(filterOptions.LineCode.ToLower()));
        }

        query = query.OrderBy(x => x.Status).ThenByDescending(x => x.CreatedDate);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<TResult> CreateTelesaleAsync(CreateTelesaleArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.UserName)) return TResult.Failed("Tên đăng nhập không được để trống!");
        if (string.IsNullOrWhiteSpace(args.Name)) return TResult.Failed("Họ tên không được để trống!");

        var existingUser = await _userManager.FindByNameAsync(args.UserName);
        if (existingUser != null) return TResult.Failed("Tên đăng nhập đã tồn tại!");

        var branch = await _context.Branches.FindAsync(args.BranchId);
        if (branch is null) return TResult.Failed("Không tìm thấy chi nhánh!");

        if (args.TeamId.HasValue)
        {
            var team = await _context.Teams.FindAsync(args.TeamId.Value);
            if (team is null) return TResult.Failed("Không tìm thấy nhóm!");
        }

        var user = new ApplicationUser
        {
            UserName = args.UserName,
            Name = args.Name,
            Email = args.Email,
            PhoneNumber = args.PhoneNumber,
            Gender = args.Gender,
            DateOfBirth = args.DateOfBirth,
            BranchId = args.BranchId,
            TeamId = args.TeamId,
            LineCode = args.LineCode,
            CreatedDate = DateTime.Now,
            Status = UserStatus.Working,
            ManagerId = args.ManagerId
        };

        var password = $"tele@{args.UserName}";
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded) return TResult.Failed(string.Join(", ", result.Errors.Select(x => x.Description)));

        var role = await _roleManager.FindByNameAsync(RoleName.Telesales);
        if (role is null)
        {
            await _roleManager.CreateAsync(new ApplicationRole
            {
                DisplayName = "Telesale",
                Name = RoleName.Telesales
            });
        }
        await _userManager.AddToRoleAsync(user, RoleName.Telesales);

        return TResult.Success;
    }

    public async Task<TResult> UpdateTelesaleAsync(UpdateTelesaleArgs args)
    {
        var user = await FindAsync(args.Id);
        if (user is null) return TResult.Failed("Không tìm thấy người dùng!");

        if (!await _userManager.IsInRoleAsync(user, RoleName.Telesales))
            return TResult.Failed("Người dùng không phải là telesale!");

        if (string.IsNullOrWhiteSpace(args.UserName)) return TResult.Failed("Tên đăng nhập không được để trống!");
        if (string.IsNullOrWhiteSpace(args.Name)) return TResult.Failed("Họ tên không được để trống!");

        if (user.UserName != args.UserName)
        {
            var existingUser = await _userManager.FindByNameAsync(args.UserName);
            if (existingUser != null) return TResult.Failed("Tên đăng nhập đã tồn tại!");
        }

        var branch = await _context.Branches.FindAsync(args.BranchId);
        if (branch is null) return TResult.Failed("Không tìm thấy chi nhánh!");

        if (args.TeamId.HasValue)
        {
            var team = await _context.Teams.FindAsync(args.TeamId.Value);
            if (team is null) return TResult.Failed("Không tìm thấy nhóm!");
        }

        user.UserName = args.UserName;
        user.Name = args.Name;
        user.Email = args.Email;
        user.PhoneNumber = args.PhoneNumber;
        user.Gender = args.Gender;
        user.DateOfBirth = args.DateOfBirth;
        user.BranchId = args.BranchId;
        user.TeamId = args.TeamId;
        user.LineCode = args.LineCode;
        user.ManagerId = args.ManagerId;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) return TResult.Failed(string.Join(", ", result.Errors.Select(x => x.Description)));

        return TResult.Success;
    }

    public async Task<TResult> DeleteTelesaleAsync(Guid id)
    {
        var user = await FindAsync(id);
        if (user is null) return TResult.Failed("Không tìm thấy người dùng!");

        if (!await _userManager.IsInRoleAsync(user, RoleName.Telesales))
            return TResult.Failed("Người dùng không phải là telesale!");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded) return TResult.Failed(string.Join(", ", result.Errors.Select(x => x.Description)));

        return TResult.Success;
    }

    public async Task<object> GetManagerOptionsAsync(SelectOptions selectOptions)
    {
        var query = from u in _context.Users
                    join ur in _context.UserRoles on u.Id equals ur.UserId
                    join r in _context.Roles on ur.RoleId equals r.Id
                    where (r.Name == RoleName.SalesManager || r.Name == RoleName.TelesaleManager) && u.Status == UserStatus.Working
                    select new
                    {
                        u.Id,
                        u.Name,
                        u.BranchId,
                        RoleName = r.Name
                    };
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        return await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync();
    }
}
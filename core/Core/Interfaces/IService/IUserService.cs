using Microsoft.AspNetCore.Identity;
using Waffle.Core.Services.Users.Args;
using Waffle.Core.Services.Users.Models;
using Waffle.Models;
using Waffle.Models.ViewModels.Users;

namespace Waffle.Core.Interfaces.IService;

public interface IUserService
{
    Task<IdentityResult> CreateAsync(CreateUserModel model);
    Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model);
    Task<TResult> AddToRoleAsync(AddToRoleModel model);
    Task<dynamic> GetUsersInRoleAsync(string roleName, UserFilterOptions filterOptions);
    Task<IdentityResult> RemoveFromRoleAsync(RemoveFromRoleModel args);
    Task<TResult<object>> DetailAsync(Guid id);
    Task<object?> GetTelesalesOptionsAsync(TelesalesSelectOptions selectOptions);
    Task<TResult> ImportAsync(UserImportArgs args);
    Task<TResult<byte[]?>> ExportAsync(UserFilterOptions filterOptions);
    Task<ListResult<object>> ListDosAsync(FilterOptions filterOptions);
    Task<TResult> SetDosAsync(SetDosArgs args);
}

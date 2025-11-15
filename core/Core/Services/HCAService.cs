using System.Security.Claims;
using Waffle.Core.Interfaces.IService;

namespace Waffle.Core.Services;

public class HCAService(IHttpContextAccessor _httpContextAccessor) : IHCAService
{
    public IEnumerable<string>? GetRoles()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null || user.Identity is null) return default;

        return user.FindAll(ClaimTypes.Role).Select(x => x.Value);
    }

    public bool IsUserInRole(string roleName)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null || user.Identity is null) return false;

        return user.IsInRole(roleName);
    }

    public bool IsUserInAnyRole(params string[] roles)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null || user.Identity is null) return false;

        return roles.Any(user.IsInRole);
    }

    public Guid GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user is null) return Guid.Empty;
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        return Guid.TryParse(userId, out var id) ? id : Guid.Empty;
    }

    public string GetUserName()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }
}

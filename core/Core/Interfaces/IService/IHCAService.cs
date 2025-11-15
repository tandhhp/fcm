namespace Waffle.Core.Interfaces.IService;

public interface IHCAService
{
    bool IsUserInRole(string roleName);
    IEnumerable<string>? GetRoles();
    bool IsUserInAnyRole(params string[] roles);
    Guid GetUserId();
    string GetUserName();
}


using Waffle.Entities.Users;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService;

public interface INotificationService
{
    Task<int> CountAsync();
    Task CreateAsync(string title, string message, Guid? userId);
    Task CreateAsync(string title, string message, List<Guid> userId);
    Task DeleteAsync(Guid id);
    Task<Notification?> GetAsync(Guid id);
    Task<ListResult<object>> MyListAsync(FilterOptions filterOptions);
}

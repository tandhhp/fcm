using Microsoft.EntityFrameworkCore;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities.Users;
using Waffle.Models;

namespace Waffle.Core.Services;

public class NotificationService(ApplicationDbContext _context, IHCAService _hcaService, ILogService logService) : INotificationService
{
    private readonly ILogService _logService = logService;

    public async Task<int> CountAsync()
    {
        var userId = _hcaService.GetUserId();
        return await _context.Notifications.CountAsync(x => x.UserId == userId && !x.IsRead);
    }

    public async Task CreateAsync(string title, string message, Guid? userId)
    {
        if (userId is null) return;
        try
        {
            var notification = new Notification
            {
                Title = title,
                Message = message,
                UserId = userId.GetValueOrDefault(),
                IsRead = false,
                CreatedAt = DateTime.Now
            };
            await _context.Notifications.AddAsync(notification);
        }
        catch (Exception ex)
        {
            await _logService.AddAsync(ex.ToString());
        }
    }

    public async Task CreateAsync(string title, string message, List<Guid> userIds)
    {
        if (!userIds.Any()) return;
        try
        {
            foreach (var userId in userIds)
            {
                var notification = new Notification
                {
                    Title = title,
                    Message = message,
                    UserId = userId,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };
                await _context.Notifications.AddAsync(notification);
            }
        }
        catch (Exception ex)
        {
            await _logService.AddAsync(ex.ToString());
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var notification = _context.Notifications.Find(id);
        if (notification != null)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Notification?> GetAsync(Guid id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification != null)
        {
            notification.IsRead = true;
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }
        return notification;
    }

    public async Task<ListResult<object>> MyListAsync(FilterOptions filterOptions)
    {
        var userId = _hcaService.GetUserId();
        var query = _context.Notifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .AsNoTracking();

        return await ListResult<object>.Success(query, filterOptions);
    }
}

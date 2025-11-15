using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers;

public class NotificationController : BaseController
{
    private readonly INotificationService _notificationService;
    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("count")]
    public async Task<IActionResult> CountAsync() => Ok(new { data = await _notificationService.CountAsync() });

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _notificationService.DeleteAsync(id);
        return Ok(new { message = "Notification deleted successfully." });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var notification = await _notificationService.GetAsync(id);
        if (notification == null)
        {
            return NotFound(new { message = "Notification not found." });
        }
        return Ok(new { data = notification });
    }

    [HttpGet("my-list")]
    public async Task<IActionResult> MyListAsync([FromQuery] FilterOptions filterOptions) => Ok(await _notificationService.MyListAsync(filterOptions));
}

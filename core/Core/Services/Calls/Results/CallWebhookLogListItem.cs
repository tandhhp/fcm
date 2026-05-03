using Waffle.Entities.Contacts;

namespace Waffle.Core.Services.Calls.Results;

public class CallWebhookLogListItem : CallWebhookLog
{
    public string? UserName { get; set; }
    public string? StaffName { get; set; }
    public string? StaffAvatar { get; set; }
    public int? BranchId { get; set; }
}

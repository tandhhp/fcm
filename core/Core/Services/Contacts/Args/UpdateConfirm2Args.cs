using Waffle.Entities.Contacts;

namespace Waffle.Core.Services.Contacts.Args;

public class UpdateConfirm2Args
{
    public Guid ContactId { get; set; }
    public Confirm2Status Confirm2Status { get; set; }
    public string? Reason { get; set; }
}

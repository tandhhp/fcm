using Waffle.Entities;

namespace Waffle.Core.Services.Contacts.Args;

public class UpdateConfirm2Args
{
    public Guid LeadId { get; set; }
    public Confirm2 Confirm2 { get; set; }
    public string? Reason { get; set; }
}

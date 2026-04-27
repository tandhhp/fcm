using Waffle.Entities;

namespace Waffle.Core.Services.Contacts.Args;

public class UpdateAttendanceScheduleArgs
{
    public Guid LeadId { get; set; }
    public string Name { get; set; } = default!;
    public DateTime EventDate { get; set; }
    public Guid EventId { get; set; }
    public string? Note { get; set; }
    public Confirm2? Confirm2 { get; set; }
}

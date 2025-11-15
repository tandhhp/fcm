using Waffle.Entities;

namespace Waffle.Models.Args;

public class ChangeStatusArgs
{
    public LeadStatus Status { get; set; }
    public Guid LeadId { get; set; }
}

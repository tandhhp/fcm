using Waffle.Entities;

namespace Waffle.Core.Services.Events.Models;

public class EventCreateArgs
{
    public string Name { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public int? CampaignId { get; set; }
}

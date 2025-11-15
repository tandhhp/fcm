using Waffle.Entities;

namespace Waffle.Core.Services.Events.Models;

public class CampaignCreateArgs
{
    public string? Name { get; set; }
    public string Code { get; set; } = default!;
    public CampaignStatus Status { get; set; }
}

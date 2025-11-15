using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Services.Events.Models;

public class CampaignFilter : FilterOptions
{
    public string? Name { get; set; }
    public CampaignStatus? Status { get; set; }
}

using Waffle.Models;

namespace Waffle.Core.Services.Events.Models;

public class VoucherSelectOptions : SelectOptions
{
    public int? CampaignId { get; set; }
}

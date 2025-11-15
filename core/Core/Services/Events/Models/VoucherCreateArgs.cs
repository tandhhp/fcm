using Waffle.Entities.Contracts;

namespace Waffle.Core.Services.Events.Models;

public class VoucherCreateArgs
{
    public string Name { get; set; } = default!;
    public int ExpiredDays { get; set; }
    public int CampaignId { get; set; }
    public string? Note { get; set; }
    public VoucherStatus Status { get; set; }
}

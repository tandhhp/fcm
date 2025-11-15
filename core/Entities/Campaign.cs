using System.ComponentModel.DataAnnotations;
using Waffle.Entities.Contracts;

namespace Waffle.Entities;

public class Campaign : AuditEntity<int>
{
    [StringLength(512)]
    public string Name { get; set; } = default!;
    [StringLength(256)]
    public string? Code { get; set; }
    public CampaignStatus Status { get; set; }

    public virtual ICollection<Event>? Events { get; set; }
    public virtual ICollection<Voucher>? Vouchers { get; set; }
}

public enum CampaignStatus
{
    Inactive = 0,
    Active = 1,
    Completed = 2
}
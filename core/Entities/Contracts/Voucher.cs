using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waffle.Entities.Contracts;

public class Voucher : BaseEntity
{
    [StringLength(256)]
    public string Name { get; set; } = default!;
    public DateTime? ActiveAt { get; set; }
    public int ExpiredDays { get; set; }
    [ForeignKey(nameof(Campaign))]
    public int? CampaignId { get; set; }
    public DateTime? ExpiredDate { get; set; }
    [StringLength(1024)]
    public string? Note { get; set; }
    public VoucherStatus Status { get; set; }

    public virtual Campaign? Campaign { get; set; }
}

public enum VoucherStatus
{
    [Display(Name = "Chưa sử dụng")]
    Unused = 0,
    [Display(Name = "Đã sử dụng")]
    Used = 1,
    [Display(Name = "Hết hạn")]
    Expired = 2,
    [Display(Name = "Đã kích hoạt")]
    Redeemed = 3,
    [Display(Name = "Đã hủy")]
    Calceled = 4,
    [Display(Name = "Đã từ chối")]
    Rejected = 5
}
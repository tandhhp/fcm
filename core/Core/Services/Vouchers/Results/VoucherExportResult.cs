using Waffle.Entities.Contracts;

namespace Waffle.Core.Services.Vouchers.Results;

public class VoucherExportResult
{
    public string? Code { get; set; }
    public string? CustomerName { get; set; }
    public string? CampaignName { get; set; }
    public DateTime? ActivedAt { get; set; }
    public DateTime? ExpiredAt { get; set; }
    public int ExpiredDays { get; set; }
    public string? CustomerPhoneNumber { get; set; }
    public string? CustomerIdNumber { get; set; }
    public VoucherStatus Status { get; set; }
    public string? Note { get; set; }
}

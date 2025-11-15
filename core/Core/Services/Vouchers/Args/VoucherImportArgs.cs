namespace Waffle.Core.Services.Vouchers.Args;

public class VoucherImportArgs
{
    public int CampaignId { get; set; }
    public IFormFile? File { get; set; }
}

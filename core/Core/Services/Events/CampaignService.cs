using Waffle.Core.Interfaces.IRepository.Events;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Interfaces.IService.Events;
using Waffle.Core.Services.Events.Models;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Services.Events;

public class CampaignService(ICampaignRepository _campaignRepository, IHCAService _hcaService) : ICampaignService
{
    public async Task<TResult> CreateAsync(CampaignCreateArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.Name)) return TResult.Failed("Vui lòng nhập tên chiến dịch!");
        await _campaignRepository.AddAsync(new Campaign
        {
            Name = args.Name,
            CreatedBy = _hcaService.GetUserId(),
            CreatedDate = DateTime.Now,
            Status = args.Status,
            Code = args.Code
        });
        return TResult.Success;
    }

    public async Task<TResult> DeleteAsync(int id)
    {
        var campaign = await _campaignRepository.FindAsync(id);
        if (campaign == null) return TResult.Failed("Chiến dịch không tồn tại!");
        if (await _campaignRepository.HasVoucherAsync(id)) return TResult.Failed("Chiến dịch đang được sử dụng, không thể xóa!");
        await _campaignRepository.DeleteAsync(campaign);
        return TResult.Success;
    }

    public Task<Campaign?> FindAsync(int id) => _campaignRepository.FindAsync(id);

    public async Task<TResult<object>> GetAsync(int id)
    {
        var campaign = await _campaignRepository.FindAsync(id);
        if (campaign == null) return TResult<object>.Failed("Chiến dịch không tồn tại!");
        return TResult<object>.Ok(new
        {
            campaign.Id,
            campaign.Name,
            campaign.Status,
            campaign.CreatedBy,
            campaign.CreatedDate,
            campaign.ModifiedBy,
            campaign.ModifiedDate
        });
    }

    public Task<ListResult<object>> ListAsync(CampaignFilter filter) => _campaignRepository.ListAsync(filter);

    public Task<object> OptionsAsync(SelectFilterOptions filterOptions) => _campaignRepository.OptionsAsync(filterOptions);

    public async Task<TResult> UpdateAsync(CampaignUpdateArgs args)
    {
        var campaign = await _campaignRepository.FindAsync(args.Id);
        if (campaign == null) return TResult.Failed("Chiến dịch không tồn tại!");
        if (string.IsNullOrWhiteSpace(args.Name)) return TResult.Failed("Vui lòng nhập tên chiến dịch!");
        campaign.Name = args.Name;
        campaign.Status = args.Status;
        campaign.Code = args.Code;
        campaign.ModifiedBy = _hcaService.GetUserId();
        campaign.ModifiedDate = DateTime.Now;
        await _campaignRepository.UpdateAsync(campaign);
        return TResult.Success;
    }
}

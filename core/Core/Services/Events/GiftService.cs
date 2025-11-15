using Waffle.Core.Interfaces.IRepository.Events;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Interfaces.IService.Events;
using Waffle.Core.Services.Events.Models;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Services.Events;

public class GiftService(IGiftRepository _giftRepository, ILogService _logService) : IGiftService
{
    public async Task<TResult> CreateAsync(GiftCreateArgs args)
    {
        await _logService.AddAsync($"Tạo mới quà tặng: {args.Name}");
        await _giftRepository.AddAsync(new Gift
        {
            Name = args.Name
        });
        return TResult.Success;
    }

    public async Task<TResult> DeleteAsync(Guid id)
    {
        var gift = await _giftRepository.FindAsync(id);
        if (gift is null) return TResult.Failed("Không tìm thấy quà tặng!");
        await _logService.AddAsync($"Xóa quà tặng: {gift.Name}");
        await _giftRepository.DeleteAsync(gift);
        return TResult.Success;
    }

    public async Task<TResult<object>> DetailAsync(Guid id)
    {
        var gift = await _giftRepository.FindAsync(id);
        if (gift is null) return TResult<object>.Failed("Không tìm thấy quà tặng!");
        return TResult<object>.Ok(new
        {
            gift.Id,
            gift.Name
        });
    }

    public Task<object?> GetOptionsAsync() => _giftRepository.GetOptionsAsync();

    public Task<ListResult<object>> ListAsync(GiftFilterOptions filterOptions) => _giftRepository.ListAsync(filterOptions);

    public async Task<TResult> UpdateAsync(GiftUpdateArgs args)
    {
        var gift = await _giftRepository.FindAsync(args.Id);
        if (gift is null) return TResult.Failed("Không tìm thấy quà tặng!");
        gift.Name = args.Name;
        await _logService.AddAsync($"Cập nhật quà tặng: {gift.Name}");
        await _giftRepository.UpdateAsync(gift);
        return TResult.Success;
    }
}

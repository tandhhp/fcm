using Waffle.Core.Interfaces.IRepository.Calls;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Interfaces.IService.Calls;
using Waffle.Core.Services.Calls.Args;
using Waffle.Core.Services.Calls.Models;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Services.Calls;

public class CallStatusService(ICallStatusRepository _callStatusRepository, ILogService _logService) : ICallStatusService
{
    public async Task<TResult> CreateAsync(CallStatusCreateArgs args)
    {
        if (await _callStatusRepository.IsExistAsync(args.Code)) return TResult.Failed("Trạng thái này đã tồn tại!");
        await _callStatusRepository.AddAsync(new CallStatus
        {
            Name = args.Name,
            Code = args.Code
        });
        return TResult.Success;
    }

    public async Task<TResult> DeleteAsync(int id)
    {
        var data = await _callStatusRepository.FindAsync(id);
        if (data == null) return TResult.Failed("Không tìm thấy trạng thái cuộc gọi");
        if (await _callStatusRepository.IsUsedAsync(id)) return TResult.Failed("Trạng thái cuộc gọi đã được sử dụng, không thể xóa!");
        await _callStatusRepository.DeleteAsync(data);
        return TResult.Success;
    }

    public Task<ListResult<object>> ListAsync(CallStatusFilterOptions filterOptions) => _callStatusRepository.ListAsync(filterOptions);

    public Task<object> OptionsAsync(SelectOptions options) => _callStatusRepository.OptionsAsync(options);

    public async Task<TResult> UpdateAsync(CallStatusUpdateArgs args)
    {
        var data = await _callStatusRepository.FindAsync(args.Id);
        if (data == null) return TResult.Failed("Không tìm thấy trạng thái cuộc gọi");
        await _logService.AddAsync($"Cập nhật trạng thái cuộc gọi {data.Name}");
        data.Name = args.Name;
        data.Code = args.Code;
        await _callStatusRepository.UpdateAsync(data);
        return TResult.Success;
    }
}

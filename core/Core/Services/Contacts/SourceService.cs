using Waffle.Core.Interfaces.IRepository.Leads;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Sources.Args;
using Waffle.Core.Services.Sources.Filters;
using Waffle.Core.Services.Sources.Results;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Services.Contacts;

public class SourceService(ISourceRepository _sourceRepository, ILogService _logService) : ISourceService
{
    public Task<TResult> AssignAsync(SourceAssignArgs args) => _sourceRepository.AssignAsync(args);

    public Task<TResult<object>> AvailablesAsync() => _sourceRepository.AvailablesAsync();

    public async Task<TResult> CreateAsync(SourceCreateArgs args)
    {
        try
        {
            if (await _sourceRepository.IsExistAsync(args.Name)) return TResult.Failed("Nguồn đã tồn tại!");
            await _logService.AddAsync($"Tạo mới nguồn: {args.Name}");
            await _sourceRepository.AddAsync(new Source
            {
                Name = args.Name
            });
            return TResult.Success;
        }
        catch (Exception ex)
        {
            await _logService.ExceptionAsync(ex);
            return TResult.Failed(ex.Message);
        }
    }

    public async Task<TResult> DeleteAsync(int id)
    {
        var data = await _sourceRepository.FindAsync(id);
        if (data == null) return TResult.Failed("Nguồn không tồn tại!");
        if (await _sourceRepository.IsUsedAsync(id)) return TResult.Failed("Nguồn đã được sử dụng, không thể xóa!");
        await _logService.AddAsync($"Xoá nguồn: {data.Name}");
        await _sourceRepository.DeleteAsync(data);
        return TResult.Success;
    }

    public async Task<TResult<object>> DetailAsync(int id)
    {
        var data = await _sourceRepository.FindAsync(id);
        if (data == null) return TResult<object>.Failed("Nguồn không tồn tại!");
        return TResult<object>.Ok(new {
            data.Id,
            data.Name
        });
    }

    public Task<Source?> FindAsync(int id) => _sourceRepository.FindAsync(id);

    public Task<ListResult<object>> ListAsync(SourceFilterOptions filterOptions) => _sourceRepository.ListAsync(filterOptions);

    public Task<object> OptionsAsync(SelectOptions selectOptions) => _sourceRepository.OptionsAsync(selectOptions);

    public Task<ListResult<SourceReportResult>> ReportAsync(FilterOptions filterOptions) => _sourceRepository.ReportAsync(filterOptions);

    public async Task<TResult> UpdateAsync(SourceUpdateArgs args)
    {
        var data = await _sourceRepository.FindAsync(args.Id);
        if (data is null) return TResult.Failed("Nguồn không tồn tại!");
        await _logService.AddAsync($"Cập nhật nguồn: {data.Name} => {args.Name}");
        data.Name = args.Name;
        await _sourceRepository.UpdateAsync(data);
        return TResult.Success;
    }
}

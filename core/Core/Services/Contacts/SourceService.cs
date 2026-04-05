using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IRepository.Leads;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Sources.Args;
using Waffle.Core.Services.Sources.Filters;
using Waffle.Core.Services.Sources.Results;
using Waffle.Entities;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Services.Contacts;

public class SourceService(ISourceRepository _sourceRepository, ILogService _logService) : ISourceService
{
    public Task<TResult> AssignAsync(SourceAssignArgs args) => _sourceRepository.AssignAsync(args);

    public Task<TResult<object>> AvailablesAsync() => _sourceRepository.AvailablesAsync();

    public Task<ListResult<object>> ContactListAsync(SourceContactFilterOptions filterOptions) => _sourceRepository.ContactListAsync(filterOptions);

    public async Task<TResult> CreateAsync(SourceCreateArgs args)
    {
        try
        {
            if (await _sourceRepository.IsExistAsync(args.Name)) return TResult.Failed("Nguồn đã tồn tại!");
            await _logService.AddAsync($"Tạo mới nguồn: {args.Name}");
            await _sourceRepository.AddAsync(new Source
            {
                Name = args.Name,
                Overwrite = args.Overwrite,
                Protected = args.Protected,
                TypeOfDataId = args.TypeOfDataId,
                TeamId = args.TeamId
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
        SourceType? sourceType = null;
        if (data.TypeOfDataId.HasValue)
        {
            var typeOfData = await _sourceRepository.GetTypeOfDataByIdAsync(data.TypeOfDataId);
            sourceType = typeOfData?.Source;
        }
        return TResult<object>.Ok(new {
            data.Id,
            data.Name,
            data.TypeOfDataId,
            data.TeamId,
            data.Protected,
            data.Overwrite,
            sourceType
        });
    }

    public Task<Source?> FindAsync(int id) => _sourceRepository.FindAsync(id);

    public Task<TResult> GetTypeOfDataBySourceIdAsync(int sourceId) => _sourceRepository.GetTypeOfDataBySourceIdAsync(sourceId);

    public Task<ListResult<object>> ListAsync(SourceFilterOptions filterOptions) => _sourceRepository.ListAsync(filterOptions);

    public Task<TResult> MultipleAssignAsync(SourceMultipleAssignArgs args) => _sourceRepository.MultipleAssignAsync(args);

    public Task<object> OptionsAsync(SourceSelectOptions selectOptions) => _sourceRepository.OptionsAsync(selectOptions);

    public Task<object?> OptionsByTypeOfDataAsync(SourceSelectOptions selectOptions) => _sourceRepository.OptionsByTypeOfDataAsync(selectOptions);

    public Task<ListResult<SourceReportResult>> ReportAsync(FilterOptions filterOptions) => _sourceRepository.ReportAsync(filterOptions);

    public Task<object?> TeamOptionsAsync(SourceTeamSelectOptions selectOptions) => _sourceRepository.TeamOptionsAsync(selectOptions);

    public Task<object?> TypeOfDataOptionsAsync(TypeOfDataSelectOptions selectOptions) => _sourceRepository.TypeOfDataOptionsAsync(selectOptions);

    public async Task<object?> TypeOfDataSourcesAsync()
    {
        var data = Enum.GetValues<SourceType>().Cast<SourceType>().Select(x => new
        {
            Value = x,
            Label = EnumHelper.GetDisplayName(x)
        }).ToList();
        return data;
    }

    public async Task<TResult> UpdateAsync(SourceUpdateArgs args)
    {
        var data = await _sourceRepository.FindAsync(args.Id);
        if (data is null) return TResult.Failed("Nguồn không tồn tại!");
        data.Name = args.Name;
        data.TeamId = args.TeamId;
        data.TypeOfDataId = args.TypeOfDataId;
        data.Protected = args.Protected;
        data.Overwrite = args.Overwrite;
        await _logService.AddAsync($"Cập nhật nguồn: {data.Name} => {args.Name}");
        await _sourceRepository.UpdateAsync(data);
        return TResult.Success;
    }
}

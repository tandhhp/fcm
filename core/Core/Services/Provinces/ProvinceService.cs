using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Provinces.Models;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Settings.Provinces;

namespace Waffle.Core.Services.Provinces;

public class ProvinceService(IProvinceRepository _provinceRepository) : IProvinceService
{
    public async Task<TResult> CreateAsync(ProvinceCreateArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.Name)) return TResult.Failed("Province name is required.");
        if (await _provinceRepository.ExistsAsync(args.Name)) return TResult.Failed("Province name already exists.");
        await _provinceRepository.AddAsync(new Province { Name = args.Name });
        return TResult.Success;
    }

    public async Task<TResult> DeleteAsync(int id)
    {
        var province = await _provinceRepository.FindAsync(id);
        if (province is null) return TResult.Failed("Province not found.");
        await _provinceRepository.DeleteAsync(province);
        return TResult.Success;
    }

    public Task<Province?> FindAsync(int provinceId) => _provinceRepository.FindAsync(provinceId);

    public Task<ListResult<object>> ListAsync(ProvinceFilterOptions filterOptions) => _provinceRepository.ListAsync(filterOptions);

    public Task<object?> OptionsAsync(string? keyWords) => _provinceRepository.OptionsAsync(keyWords);

    public async Task<TResult> UpdateAsync(ProvinceUpdateArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.Name)) return TResult.Failed("Province name is required.");
        var province = await _provinceRepository.FindAsync(args.Id);
        if (province is null) return TResult.Failed("Province not found.");
        province.Name = args.Name;
        await _provinceRepository.UpdateAsync(province);
        return TResult.Success;
    }
}

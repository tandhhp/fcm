using Waffle.Core.Services.Provinces.Models;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Settings.Provinces;

namespace Waffle.Core.Interfaces.IService;

public interface IProvinceService
{
    Task<TResult> CreateAsync(ProvinceCreateArgs args);
    Task<TResult> DeleteAsync(int id);
    Task<ListResult<object>> ListAsync(ProvinceFilterOptions filterOptions);
    Task<object?> OptionsAsync(string? keyWords);
    Task<TResult> UpdateAsync(ProvinceUpdateArgs args);
    Task<Province?> FindAsync(int provinceId);
}

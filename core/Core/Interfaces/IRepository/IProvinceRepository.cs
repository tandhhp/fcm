using Waffle.Core.Services.Provinces.Models;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository;

public interface IProvinceRepository : IAsyncRepository<Province>
{
    Task<bool> ExistsAsync(string name);
    Task<ListResult<object>> ListAsync(ProvinceFilterOptions filterOptions);
    Task<object?> OptionsAsync(string? keyWords);
}

using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Districts.Models;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Services.Districts;

public class DistrictService(IDistrictRepository _districtRepository) : IDistrictService
{
    public async Task<TResult> CreateAsync(CreateDistrictArgs args)
    {
        await _districtRepository.AddAsync(new District
        {
            Name = args.Name,
            ProvinceId = args.ProvinceId
        });
        return TResult.Success;
    }

    public async Task<TResult> DeleteAsync(int id)
    {
        var district = await _districtRepository.FindAsync(id);
        if (district == null)
        {
            return TResult.Failed("District not found.");
        }
        await _districtRepository.DeleteAsync(district);
        return TResult.Success;
    }

    public Task<District?> FindAsync(int id) => _districtRepository.FindAsync(id);

    public Task<District?> FindByNameAsync(string? name) => _districtRepository.FindByNameAsync(name);

    public Task<ListResult<object>> ListAsync(DistrictFilterOptions filterOptions) => _districtRepository.ListAsync(filterOptions);

    public Task<object> OptionsAsync(DistrictSelectOptions selectOptions) => _districtRepository.OptionsAsync(selectOptions);

    public async Task<TResult> UpdateAsync(UpdateDistrictArgs args)
    {
        var district = await _districtRepository.FindAsync(args.Id);
        if (district == null)
        {
            return TResult.Failed("District not found.");
        }
        district.Name = args.Name;
        district.ProvinceId = args.ProvinceId;
        await _districtRepository.UpdateAsync(district);
        return TResult.Success;
    }
}

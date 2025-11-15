using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Services.Districts.Models;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories;

public class DistrictRepository(ApplicationDbContext context) : EfRepository<District>(context), IDistrictRepository
{
    public async Task<District?> FindByNameAsync(string? name) => await _context.Districts.FirstOrDefaultAsync(d => d.Name == name);

    public async Task<ListResult<object>> ListAsync(DistrictFilterOptions filterOptions)
    {
        var query = from d in _context.Districts
                    select new
                    {
                        d.Id,
                        d.Name,
                        d.ProvinceId
                    };
        if (filterOptions.ProvinceId != null)
        {
            query = query.Where(d => d.ProvinceId == filterOptions.ProvinceId);
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(d => d.Name.Contains(filterOptions.Name));
        }
        query = query.OrderBy(d => d.Name);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<object> OptionsAsync(DistrictSelectOptions selectOptions)
    {
        var query = from d in _context.Districts
                    select new
                    {
                        d.Id,
                        d.Name,
                        d.ProvinceId
                    };
        if (selectOptions.ProvinceId != null)
        {
            query = query.Where(d => d.ProvinceId == selectOptions.ProvinceId);
        }
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(d => d.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        return await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync();
    }
}

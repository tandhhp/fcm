using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Services.Provinces.Models;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories;

public class ProvinceRepository(ApplicationDbContext context) : EfRepository<Province>(context), IProvinceRepository
{
    public async Task<bool> ExistsAsync(string name) => await _context.Provinces.AnyAsync(p => p.Name == name);

    public async Task<ListResult<object>> ListAsync(ProvinceFilterOptions filterOptions)
    {
        var query = from p in _context.Provinces
                    select new
                    {
                        p.Id,
                        p.Name,
                        DistrictCount = _context.Districts.Count(d => d.ProvinceId == p.Id)
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(p => p.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        query = query.OrderBy(p => p.Name);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<object?> OptionsAsync(string? keyWords)
    {
        var query = from p in _context.Provinces
                    select new
                    {
                        p.Id,
                        p.Name
                    };
        if (!string.IsNullOrWhiteSpace(keyWords))
        {
            query = query.Where(p => p.Name.ToLower().Contains(keyWords.ToLower()));
        }
        query = query.OrderBy(p => p.Name);
        return await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync();
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Components;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Waffle.Core.Services;

public class ComponentService : IComponentService
{
    private readonly ApplicationDbContext _context;
    private readonly IComponentRepository _componentRepository;

    public ComponentService(ApplicationDbContext context, IComponentRepository componentRepository)
    {
        _context = context;
        _componentRepository = componentRepository;
    }

    public async Task<IdentityResult> ActiveAsync(Guid id)
    {
        var component = await _context.Components.FindAsync(id);
        if (component == null)
        {
            return IdentityResult.Failed();
        }
        component.Active = !component.Active;
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> AddAsync(Component args)
    {
        if (args == null || string.IsNullOrEmpty(args.NormalizedName))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Missing params!"
            });
        }
        if (await _context.Components.AnyAsync(x => x.NormalizedName.Equals(args.NormalizedName)))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Component exist!"
            });
        }
        await _context.Components.AddAsync(args);
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(Guid id)
    {
        var component = await FindAsync(id);
        if (component is null)
        {
            return IdentityResult.Failed();
        }
        if (await HasWorkContentAsync(id))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Has work content!"
            });
        }
        _context.Remove(component);
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<Component> EnsureComponentAsync(string normalizedName)
    {
        var component = await _context.Components.FirstOrDefaultAsync(x => x.NormalizedName.ToLower().Equals(normalizedName.ToLower()));
        if (component is null)
        {
            component = new Component
            {
                NormalizedName = normalizedName,
                Name = normalizedName,
                Active = true
            };
            await _context.Components.AddAsync(component);
            await _context.SaveChangesAsync();
        }
        return component;
    }

    public async Task<Component?> FindAsync(Guid id) => await _context.Components.FindAsync(id);

    private static IEnumerable<Option> ToFormSelect(List<Component> components)
    {
        foreach (var item in components)
        {
            var display = ComponentHelper.GetNormalizedName(item.NormalizedName);
            if (display is null) continue;
            var filter = display.GetAutoGenerateFilter() ?? false;
            if (filter) continue;

            yield return new Option
            {
                Label = item.Name,
                Value = item.Id
            };
        }
    }

    public async Task<IEnumerable<Option>> FormSelectAsync(SearchFilterOptions filterOptions)
    {
        filterOptions.SearchTerm = SeoHelper.ToSeoFriendly(filterOptions.SearchTerm);
        var components = await _componentRepository.ListAsync(filterOptions);
        return ToFormSelect(components);
    }

    public async Task<Component?> GetByNameAsync(string name)
    {
        if (string.IsNullOrEmpty(name)) return default;
        return await _context.Components.FirstOrDefaultAsync(x => x.NormalizedName.ToLower().Equals(name.ToLower()));
    }

    public async Task<bool> HasWorkContentAsync(Guid id) => await _context.WorkContents.AnyAsync(x => x.ComponentId == id);

    public async Task<IEnumerable<Component>> ListAllAsync() => await _context.Components.Where(x => x.Active).OrderBy(x => x.NormalizedName).ToListAsync();

    public Task<ListResult<Component>> ListAsync(ComponentFilterOptions filterOptions) => _componentRepository.ListAsync(filterOptions);

    public async Task<ListResult<WorkListItem>> ListWorkAsync(Guid id, WorkFilterOptions filterOptions)
    {
        var query = from a in _context.WorkContents
                    join b in _context.Components on a.ComponentId equals b.Id
                    where a.ComponentId == id
                    orderby a.Id ascending
                    select new WorkListItem
                    {
                        Active = a.Active,
                        Name = a.Name,
                        NormalizedName = b.NormalizedName,
                        Id = a.Id
                    };
        return await ListResult<WorkListItem>.Success(query, filterOptions);
    }

    public async Task<IdentityResult> UpdateAsync(Component args)
    {
        var component = await FindAsync(args.Id);
        if (component is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Data not found"
            });
        }
        component.Active = args.Active;
        component.Name = args.Name;
        component.NormalizedName = args.NormalizedName;
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }
}

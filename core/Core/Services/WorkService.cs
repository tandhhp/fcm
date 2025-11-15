using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Components;
using Waffle.Models.Params.Products;

namespace Waffle.Core.Services;

public class WorkService : IWorkService
{
    private readonly ApplicationDbContext _context;
    private readonly IComponentService _componentService;
    private readonly IComponentRepository _componentRepository;
    private readonly IWorkContentRepository _workRepository;
    private readonly ILogger<WorkService> _logger;

    public WorkService(ApplicationDbContext context, IComponentService componentService, IWorkContentRepository workContentRepository, IComponentRepository componentRepository, ILogger<WorkService> logger)
    {
        _context = context;
        _componentService = componentService;
        _workRepository = workContentRepository;
        _componentRepository = componentRepository;
        _logger = logger;
    }

    public async Task<IdentityResult> ActiveAsync(Guid id)
    {
        var workItem = await FindAsync(id);
        if (workItem is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Work item not found!"
            });
        }
        workItem.Active = !workItem.Active;
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> ColumnAddAsync(Column column)
    {
        var component = await _componentService.EnsureComponentAsync(nameof(Column));
        var workContent = new WorkContent
        {
            Active = true,
            Arguments = JsonSerializer.Serialize(column),
            ComponentId = component.Id,
            ParentId = column.RowId,
            Name = column.Name ?? column.ClassName
        };
        await _context.WorkContents.AddAsync(workContent);
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(Guid id)
    {
        var workContent = await FindAsync(id);
        if (workContent is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Work not found!"
            });
        }
        if (await _context.WorkContents.AnyAsync(x => x.ParentId == id))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Please remove child"
            });
        }
        var workItems = await _context.WorkItems.Where(x => x.WorkId == id).ToListAsync();
        _context.WorkItems.RemoveRange(workItems);
        _context.WorkContents.Remove(workContent);
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<dynamic> ExportByCatalogAsync(Guid catalogId)
    {
        var query = from workItems in _context.WorkItems
                    join workContents in _context.WorkContents on workItems.WorkId equals workContents.Id
                    select new { workItems, workContents };
        return await query.ToListAsync();
    }

    public async Task<WorkContent?> FindAsync(Guid? id)
    {
        if (id == null) return default;
        return await _workRepository.FindAsync(id);
    }

    public async Task<T?> GetAsync<T>(Guid? id)
    {
        var work = await FindAsync(id);
        return Get<T>(work?.Arguments);
    }

    public T? Get<T>(string? arguments)
    {
        if (string.IsNullOrEmpty(arguments))
        {
            _logger.LogError("Arguments empty!");
            return default;
        }
        return JsonSerializer.Deserialize<T>(arguments);
    }

    public async Task<IEnumerable<Option>> GetListAsync(BasicFilterOptions filterOptions)
    {
        var query = from a in _context.Components
                    join b in _context.WorkContents on a.Id equals b.ComponentId
                    select new Option
                    {
                        Label = $"[{a.Name}] {b.Name}",
                        Value = b.Id
                    };
        return await query.ToListAsync();
    }

    public async Task<IdentityResult> ItemAddAsync(WorkItem args)
    {
        if (await _context.WorkItems.AnyAsync(x => x.CatalogId == args.CatalogId && x.WorkId == args.WorkId))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Dupplicate data"
            });
        }
        await _context.WorkItems.AddAsync(args);
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<ListResult<WorkListItem>> GetWorkListItemChildAsync(WorkFilterOptions filterOptions)
    {
        var query = from b in _context.WorkContents
                    join c in _context.Components on b.ComponentId equals c.Id
                    where (filterOptions.ParentId == null || b.ParentId == filterOptions.ParentId) &&
                    (filterOptions.Active == null || b.Active == filterOptions.Active)
                    orderby b.SortOrder ascending
                    select new WorkListItem
                    {
                        Id = b.Id,
                        Name = b.Name,
                        NormalizedName = c.NormalizedName,
                        Active = b.Active
                    };
        var data = await query.ToListAsync();
        foreach (var item in data)
        {
            var display = ComponentHelper.GetNormalizedName(item.NormalizedName);
            item.NormalizedName = display?.GetPrompt() ?? item.NormalizedName;
            item.AutoGenerateField = display?.GetAutoGenerateField() ?? false;
        }
        return ListResult<WorkListItem>.Success(data, filterOptions);
    }

    public async Task<IdentityResult> NavbarSettingSaveAsync(Navbar args)
    {
        var navbar = await GetAsync<Navbar>(args.Id);
        if (navbar == null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Navbar not found"
            });
        }
        navbar.Layout = args.Layout;
        var work = await FindAsync(args.Id);
        if (work == null) return IdentityResult.Failed();
        work.Arguments = JsonSerializer.Serialize(navbar);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> SaveColumnAsync(Column item)
    {
        var work = await FindAsync(item.Id);
        if (work is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Work not found!"
            });
        }
        work.Arguments = JsonSerializer.Serialize(item);
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> SaveContactFormAsync(ContactForm item)
    {
        var workContent = await FindAsync(item.Id);
        if (workContent is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Work content not found!"
            });
        }
        workContent.Arguments = JsonSerializer.Serialize(item);
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> SaveRowAsync(Row row)
    {
        var workContent = await FindAsync(row.Id);
        if (workContent is null)
        {
            return IdentityResult.Failed();
        }
        workContent.Arguments = JsonSerializer.Serialize(row);
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> SaveTagAsync(Tag tag)
    {
        var work = await FindAsync(tag.Id);
        if (work is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Data not found!"
            });
        }
        work.Arguments = JsonSerializer.Serialize(tag);
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<IEnumerable<Option>> TagListAsync(WorkFilterOptions filterOptions)
    {
        var query = from a in _context.Components
                    join b in _context.WorkContents on a.Id equals b.ComponentId
                    where a.NormalizedName.Equals(nameof(Tag))
                    orderby b.Name ascending
                    select new Option
                    {
                        Label = b.Name,
                        Value = b.Id
                    };
        return await query.Skip((filterOptions.Current - 1) * filterOptions.PageSize).Take(filterOptions.PageSize).ToListAsync();
    }

    public async Task<IEnumerable<WorkContent>> GetWorkContentChildsAsync(Guid parentId)
    {
        return await _context.WorkContents.Where(x => x.Active && x.ParentId == parentId).OrderByDescending(x => x.Id).ToListAsync();
    }

    public async Task<IdentityResult> SaveAsync(WorkContent args)
    {
        var work = await FindAsync(args.Id);
        if (work == null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "general.dataNotFound",
                Description = "Work not found"
            });
        }
        work.Active = args.Active;
        work.Name = args.Name;
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task AddAsync(WorkContent workContent)
    {
        if (string.IsNullOrEmpty(workContent.Name))
        {
            var component = await _componentRepository.FindAsync(workContent.ComponentId);
            if (component != null)
            {
                workContent.Name = component.Name;
            }
        }
        await _context.AddAsync(workContent);
        await _context.SaveChangesAsync();
    }

    public async Task AddItemAsync(Guid workId, Guid catalogId)
    {
        await _context.WorkItems.AddAsync(new WorkItem
        {
            CatalogId = catalogId,
            WorkId = workId
        });
        await _context.SaveChangesAsync();
    }

    public async Task<IdentityResult> SaveArgumentsAsync(Guid id, object args)
    {
        var work = await FindAsync(id);
        if (work == null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Data not found"
            });
        }
        work.Arguments = JsonSerializer.Serialize(args);
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<WorkContent?> GetSummaryAsync(Guid id)
    {
        return await _context.WorkContents.Select(x => new WorkContent
        {
            Active = x.Active,
            Name = x.Name,
            ComponentId = x.ComponentId,
            Id = x.Id,
            ParentId = x.ParentId
        }).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IdentityResult> UpdateSummaryAsync(WorkContent args)
    {
        var work = await FindAsync(args.Id);
        if (work == null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Data not found!"
            });
        }
        work.Active = args.Active;
        work.Name = args.Name;
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> ItemDeleteAsync(WorkItem args)
    {
        var data = await _context.WorkItems.FirstOrDefaultAsync(x => x.CatalogId == args.CatalogId && x.WorkId == args.WorkId);
        if (data is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = $"Data not found: WorkId ({args.WorkId}) / CatalogId ({args.CatalogId})"
            });
        }
        if (await _context.WorkItems.CountAsync(x => x.WorkId == args.WorkId) == 1)
        {
            var work = await FindAsync(args.WorkId);
            if (work != null)
            {
                _context.WorkContents.Remove(work);
            }
        }

        _context.WorkItems.Remove(data);
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<List<T>> GetListChildAsync<T>(Guid parentId)
    {
        var r = new List<T>();
        var works = await _context.WorkContents.Where(x => x.ParentId == parentId && x.Active).ToListAsync();
        foreach (var work in works)
        {
            if (string.IsNullOrEmpty(work.Arguments))
            {
                continue;
            }
            var w = Get<T>(work.Arguments);
            if (w != null)
            {
                r.Add(w);
            }
        }
        return r;
    }

    public IEnumerable<T?> ListAsync<T>(List<string> list)
    {
        foreach (var item in list)
        {
            if (string.IsNullOrEmpty(item))
            {
                continue;
            }
            yield return JsonSerializer.Deserialize<T>(item);
        }
    }

    public async Task<ListResult<WorkListItem>> ListBySettingIdAsync(Guid id)
    {
        var query = from a in _context.WorkContents
                    join b in _context.Components on a.ComponentId equals b.Id
                    where a.ParentId == id
                    select new WorkListItem
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Active = a.Active,
                        NormalizedName = b.NormalizedName,
                    };
        return await ListResult<WorkListItem>.Success(query, new BasicFilterOptions());
    }

    public async Task<object?> GetListUnuseAsync(BasicFilterOptions filterOptions)
    {
        var query = from a in _context.WorkContents
                    join b in _context.WorkItems on a.Id equals b.WorkId into g
                    from g2 in g.DefaultIfEmpty()
                    where g2 == null && (a.ParentId == null || a.ParentId == Guid.Empty)
                    select a;
        return new
        {
            data = await query.ToListAsync()
        };
    }

    public async Task<IdentityResult> SaveProductImageAsync(SaveImageModel args)
    {
        try
        {
            var component = await _componentRepository.FindByNameAsync(nameof(ProductImage));
            if (component is null)
            {
                _logger.LogError("Component {Name} not found", nameof(ProductImage));
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "error.componentNotFound",
                    Description = "Component not found!"
                });
            }
            var work = await _workRepository.FindByCatalogAsync(args.CatalogId, component.Id);
            var productImage = new ProductImage
            {
                Images = args.Urls
            };
            if (work != null)
            {
                work.Arguments = JsonSerializer.Serialize(productImage);
                await _workRepository.SaveChangesAsync();
                return IdentityResult.Success;
            }
            work = new WorkContent
            {
                Active = true,
                ComponentId = component.Id,
                Name = string.Empty,
                Arguments = JsonSerializer.Serialize(productImage)
            };
            await _workRepository.AddAsync(work);

            await _workRepository.AddItemAsync(args.CatalogId, work.Id);

            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = nameof(Exception),
                Description = ex.ToString()
            });
        }
    }

    public async Task<IEnumerable<WorkListItem>> GetComponentsInColumnAsync(Guid workId)
    {
        var query = from b in _context.WorkContents
                    join c in _context.Components on b.ComponentId equals c.Id
                    orderby b.SortOrder ascending
                    where b.ParentId == workId && b.Active
                    select new WorkListItem
                    {
                        Id = b.Id,
                        Name = b.Name,
                        NormalizedName = c.NormalizedName,
                        Active = b.Active
                    };
        return await query.ToListAsync();
    }

    public async IAsyncEnumerable<Column> ListColumnAsync(Guid rowId)
    {
        var works = await _workRepository.ListChildAsync(rowId);
        foreach (var work in works)
        {
            if (string.IsNullOrEmpty(work.Arguments)) yield return new Column();
            var column = Get<Column>(work.Arguments);
            if (column is null) yield return new Column();
            var items = await GetWorkListItemChildAsync(new WorkFilterOptions
            {
                ParentId = work.Id
            });
            column!.Id = work.Id;
            column!.Items = items.Data;
            yield return column;
        }
    }

    public Task<IEnumerable<Guid>> ListChildIdAsync(Guid parentId) => _workRepository.ListChildIdAsync(parentId);

    public async Task<object?> GetUnuseWorksAsync(SearchFilterOptions filterOptions)
    {
        var searchTerm = SeoHelper.ToSeoFriendly(filterOptions.SearchTerm);
        var query = from i in _context.WorkItems
                    join c in _context.Catalogs on i.CatalogId equals c.Id
                    join w in _context.WorkContents on i.WorkId equals w.Id
                    into iw
                    from w in iw.DefaultIfEmpty()
                    join b in _context.Components on w.ComponentId equals b.Id
                    into bw
                    from b in bw.DefaultIfEmpty()
                    where w == null && c.Type != CatalogType.Tag && (string.IsNullOrEmpty(filterOptions.SearchTerm) || c.NormalizedName.Contains(searchTerm))
                    select new
                    {
                        i.WorkId,
                        i.CatalogId,
                        catalogName = c.Name,
                        workName = w.Name,
                        componentName = b.Name
                    };
        return new
        {
            total = await query.CountAsync(),
            data = await query.OrderBy(x => x.WorkId).Skip((filterOptions.Current - 1) * filterOptions.PageSize).Take(filterOptions.PageSize).ToListAsync()
        };
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Waffle.Core.Constants;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Foundations;
using Waffle.Models;
using Waffle.Models.Components;
using Waffle.Models.Components.Specifications;
using Waffle.Models.Params;
using Column = Waffle.Models.Components.Column;

namespace Waffle.Controllers;

public class WorkController : BaseController
{
    private readonly ApplicationDbContext _context;
    private readonly IFileService _fileContentService;
    private readonly IWorkService _workService;
    private readonly IComponentService _componentService;
    private readonly ICatalogService _catalogService;
    private readonly ILogService _logService;

    public WorkController(ApplicationDbContext context, IFileService fileContentService, IWorkService workContentService, IComponentService componentService, ICatalogService catalogService, ILogService logService)
    {
        _context = context;
        _fileContentService = fileContentService;
        _workService = workContentService;
        _componentService = componentService;
        _catalogService = catalogService;
        _logService = logService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddAsync([FromBody] AddWorkContentModel args)
    {
        var workContent = new WorkContent
        {
            Name = args.Name,
            ComponentId = args.ComponentId,
            Active = true
        };
        await _workService.AddAsync(workContent);
        await _workService.AddItemAsync(workContent.Id, args.CatalogId);
        return Ok(IdentityResult.Success);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id) => Ok(await _workService.FindAsync(id));

    [HttpGet("summary/{id}")]
    public async Task<IActionResult> GetSummaryAsync([FromRoute] Guid id) => Ok(await _workService.GetSummaryAsync(id));

    [HttpPost("summary/update")]
    public async Task<IActionResult> UpdateSummaryAsync([FromBody] WorkContent args) => Ok(await _workService.UpdateSummaryAsync(args));

    [HttpGet("list-unuse")]
    public async Task<IActionResult> GetListUnuseAsync([FromQuery] BasicFilterOptions filterOptions) => Ok(await _workService.GetListUnuseAsync(filterOptions));

    [HttpGet("unuse-works")]
    public async Task<IActionResult> GetUnuseWorksAsync([FromQuery] SearchFilterOptions filterOptions) => Ok(await _workService.GetUnuseWorksAsync(filterOptions));

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync([FromQuery] BasicFilterOptions filterOptions) => Ok(await _workService.GetListAsync(filterOptions));

    [HttpGet("list/{id}")]
    public async Task<IActionResult> ListAsync([FromRoute] Guid id)
    {
        var query = from catalog in _context.Catalogs
                    join b in _context.WorkItems on catalog.Id equals b.CatalogId
                    join c in _context.WorkContents on b.WorkId equals c.Id
                    join component in _context.Components on c.ComponentId equals component.Id
                    where catalog.Id == id
                    orderby b.SortOrder ascending
                    select new WorkListItem
                    {
                        Id = c.Id,
                        Name = $"[{component.Name}] {c.Name}",
                        SortOrder = b.SortOrder,
                        CatalogId = catalog.Id,
                        Active = c.Active,
                        NormalizedName = component.NormalizedName
                    };
        var data = await query.ToListAsync();
        foreach (var item in data)
        {
            var display = ComponentHelper.GetNormalizedName(item.NormalizedName);
            item.NormalizedName = display?.GetPrompt() ?? item.NormalizedName;
            item.AutoGenerateField = display?.GetAutoGenerateField() ?? false;
        }
        return Ok(ListResult<WorkListItem>.Success(data, new BasicFilterOptions()));
    }

    [HttpPost("item/add")]
    public async Task<IActionResult> ItemAddAsync([FromBody] WorkItem args) => Ok(await _workService.ItemAddAsync(args));

    [HttpPost("item/delete")]
    public async Task<IActionResult> ItemDeleteAsync([FromBody] WorkItem args)
    {
        var result = await _workService.ItemDeleteAsync(args);
        if (result.Succeeded)
        {
            await _logService.AddAsync("Delete work");
        }
        return Ok(result);
    }

    [HttpPost("child/add")]
    public async Task<IActionResult> AddChildAsync([FromBody] WorkContent workContent)
    {
        if (!await _context.WorkContents.AnyAsync(x => x.Id == workContent.ParentId)) return BadRequest("Parent not found!");
        await _workService.AddAsync(workContent);
        return Ok(IdentityResult.Success);
    }

    [HttpPost("save"), Authorize(Roles = RoleName.Admin)]
    public async Task<IActionResult> SaveAsync([FromBody] WorkContent args) => Ok(await _workService.SaveAsync(args));

    [HttpPost("save/{id}")]
    public async Task<IActionResult> SaveArgumentsAsync([FromRoute] Guid id, [FromBody] object args) => Ok(await _workService.SaveArgumentsAsync(id, args));

    [HttpGet("arguments/{id}")]
    public async Task<IActionResult> GetArgumentsAsync([FromRoute] Guid id) => Ok(await _workService.GetAsync<object>(id));

    [HttpPost("active/{id}")]
    public async Task<IActionResult> ActiveAsync([FromRoute] Guid id) => Ok(await _workService.ActiveAsync(id));

    [HttpPost("delete"), Authorize(Roles = RoleName.Admin)]
    public async Task<IActionResult> DeleteAsync([FromBody] DeleteWorkContent model)
    {
        var workItem = await _context.WorkItems.FirstOrDefaultAsync(x => x.WorkId == model.WorkContentId && x.CatalogId == model.CatalogId);
        if (workItem is null) return BadRequest("Work item not found!");

        if (await _context.WorkContents.AnyAsync(x => x.ParentId == model.WorkContentId))
        {
            return Ok(IdentityResult.Failed(new IdentityError { Description = "Please remove child items first!" }));
        }

        if (await _context.WorkItems.CountAsync(x => x.WorkId == model.WorkContentId) == 1)
        {
            var workContent = await _workService.FindAsync(model.WorkContentId);
            if (workContent is null) return BadRequest("Work not found!");
            _context.WorkContents.Remove(workContent);
        }
        _context.WorkItems.Remove(workItem);
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id) => Ok(await _workService.DeleteAsync(id));

    [HttpGet("fetch-url")]
    public async Task<IActionResult> FetchUrlAsync([FromQuery] string url)
    {
        using var http = new HttpClient();
        var response = await http.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var html = await response.Content.ReadAsStringAsync();
        }
        return Ok(new {
            success = 1,
            meta = new {
                title = "",
                description = "",
                image = new {
                    url = ""
                }
            }
        });
    }

    [HttpGet("child/list/{id}")]
    public async Task<IActionResult> GetChildListAsync([FromRoute] Guid id) => Ok(await _workService.GetWorkListItemChildAsync(new WorkFilterOptions
    {
        ParentId = id
    }));

    [HttpGet("navbar/{id}")]
    public async Task<IActionResult> GetNavbarAsync([FromRoute] Guid id) => Ok(await _workService.GetAsync<Navbar>(id));

    [HttpPost("navbar/save")]
    public async Task<IActionResult> SaveNavbarAsync([FromBody] Navbar model)
    {
        var workContent = await _workService.FindAsync(model.Id);
        if (workContent is null) return BadRequest("Work not found!");
        workContent.Arguments = JsonSerializer.Serialize(model);
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpPost("navbar/item/add")]
    public async Task<IActionResult> AddNavbarItemAsync([FromBody] WorkContent args)
    {
        var parrent = await _workService.FindAsync(args.ParentId ?? Guid.Empty);
        if (parrent is null)
        {
            return Ok(IdentityResult.Failed(new IdentityError
            {
                Description = "Navbar not found"
            }));
        }
        var component = await _componentService.EnsureComponentAsync(nameof(NavItem));
        args.ComponentId = component.Id;
        await _workService.AddAsync(args);
        return Ok(IdentityResult.Success);
    }

    [HttpGet("navbar/item/list/{id}")]
    public async Task<IActionResult> ListNavItemAsync([FromRoute] Guid id)
    {
        var works = await _workService.GetAsync<NavItem>(id) ?? new NavItem();
        return Ok(new
        {
            data = works.Links
        });
    }

    [HttpPost("navbar/item/save/{id}")]
    public async Task<IActionResult> SaveNavbarItemAsync([FromRoute] Guid id, [FromBody] Link args)
    {
        args.Id = Guid.NewGuid();
        var navItem = await _workService.GetAsync<NavItem>(id) ?? new NavItem();
        if (navItem.Links.Any(x => x.Href.Equals(args.Href)))
        {
            return Ok(IdentityResult.Failed(new IdentityError
            {
                Description = $"{nameof(Link)} exist"
            }));
        }
        navItem.Links.Add(args);
        return Ok(await _workService.SaveArgumentsAsync(id, navItem));
    }

    [HttpPost("navbar/item/delete")]
    public async Task<IActionResult> DeleteNavItemAsync([FromBody] DeleteNavItemModel args)
    {
        var work = await _workService.GetAsync<NavItem>(args.WorkId);
        if (work is null)
        {
            return Ok(IdentityResult.Failed(new IdentityError
            {
                Description = $"{nameof(NavItem)} not found"
            }));
        }
        work.Links = work.Links.Where(x => x.Id != args.LinkId).ToList();
        return Ok(await _workService.SaveArgumentsAsync(args.WorkId, work));
    }

    [HttpPost("navbar/setting/save")]
    public async Task<IActionResult> NavbarSettingSaveAsync([FromBody] Navbar args) => Ok(await _workService.NavbarSettingSaveAsync(args));

    [HttpPost("contact-form/save")]
    public async Task<IActionResult> SaveContactFormAsync([FromBody] ContactForm model) => Ok(await _workService.SaveContactFormAsync(model));

    [HttpGet("row/{id}")]
    public async Task<IActionResult> GetRowAsync([FromRoute] Guid id) => Ok(await _workService.GetAsync<Row>(id));

    [HttpPost("row/save")]
    public async Task<IActionResult> SaveRowAsync([FromBody] Row row) => Ok(await _workService.SaveRowAsync(row));

    [HttpGet("contact-form/{id}")]
    public async Task<IActionResult> GetContactFormAsync([FromRoute] Guid id) => Ok(await _workService.GetAsync<ContactForm>(id));

    [HttpPost("column/add")]
    public async Task<IActionResult> ColumnAddAsync([FromBody] Column column)
    {
        var row = await _workService.FindAsync(column.RowId);
        if (row is null) return BadRequest("Row not found!");
        return Ok(await _workService.ColumnAddAsync(column));
    }

    [HttpGet("column/list/{id}")]
    public async Task<IActionResult> GetColumnListAsync([FromRoute] Guid id)
    {
        var row = await _workService.FindAsync(id);
        if (row is null) return BadRequest("Row not found!");
        return Ok(_workService.ListColumnAsync(id));
    }

    [HttpGet("column/{id}")]
    public async Task<IActionResult> GetColumnAsync([FromRoute] Guid id) => Ok(await _workService.GetAsync<Column>(id));

    [HttpPost("column/save")]
    public async Task<IActionResult> SaveColumnAsync([FromBody] Column item) => Ok(await _workService.SaveColumnAsync(item));

    [HttpGet("block-editor/{id}")]
    public async Task<IActionResult> GetBlockEditorAsync([FromRoute] Guid id) => Ok(await _workService.GetAsync<List<BlockEditorBlock>>(id));

    [HttpGet("block-editor/fetch-url")]
    public async Task<IActionResult> BlockEditorFetchUrlAsync([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return Ok(new
            {
                success = false
            });
        }
        if (url.Contains(Request.Host.Value, StringComparison.OrdinalIgnoreCase))
        {
            var normalizedName = url[(url.LastIndexOf("/") + 1)..];
            var catalog = await _catalogService.GetByNameAsync(normalizedName);
            if (catalog is null)
            {
                return Ok(new
                {
                    success = false
                });
            }
            return Ok(new
            {
                success = true,
                link = $"/{catalog.Type.ToString().ToLower()}/{normalizedName}",
                meta = new
                {
                    title = catalog.Name,
                    description = catalog.Description,
                    image = new
                    {
                        url = catalog.Thumbnail
                    }
                }
            });
        }
        return Ok(new
        {
            success = true,
            link = url,
            meta = new
            {
                title = url,
            }
        });
    }

    [HttpPost("block-editor/save")]
    public async Task<IActionResult> SaveBlockEditorAsync([FromBody] BlockEditor model)
    {
        var workContent = await _workService.FindAsync(model.Id);
        if (workContent is null)
        {
            return BadRequest();
        }
        workContent.Arguments = JsonSerializer.Serialize(model.Blocks);
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpGet("card/{id}")]
    public async Task<IActionResult> GetCardAsync([FromRoute] Guid id) => Ok(await _workService.GetAsync<Card>(id));

    [HttpPost("lookbook/add")]
    public async Task<IActionResult> AddLookBookAsync([FromBody] WorkContent model)
    {
        var workContent = await _workService.FindAsync(model.Id);
        if (workContent is null)
        {
            return Ok(IdentityResult.Failed(new IdentityError
            {
                Description = "Work content not found!"
            }));
        }
        var image = await _componentService.GetByNameAsync(nameof(Image));
        if (image is null)
        {
            return Ok(IdentityResult.Failed(new IdentityError
            {
                Description = "Image component not found!"
            }));
        }
        await _workService.AddAsync(new WorkContent
        {
            Active = true,
            ComponentId = image.Id,
            Name = model.Name,
            ParentId = workContent.Id
        });
        return Ok(IdentityResult.Success);
    }

    [HttpGet("tag/list")]
    public async Task<IActionResult> TagListAsync(WorkFilterOptions filterOptions) => Ok(await _workService.TagListAsync(filterOptions));

    [HttpPost("tag/save")]
    public async Task<IActionResult> SaveTagAsync([FromBody] Tag tag) => Ok(await _workService.SaveTagAsync(tag));

    [HttpPost("link/save/{id}")]
    public async Task<IActionResult> SaveLinkAsync([FromRoute] Guid id, [FromBody] Link args) => Ok(await _workService.SaveArgumentsAsync(id, args));

    [HttpGet("link/{id}")]
    public async Task<IActionResult> GetLinkAsync([FromRoute] Guid id) => Ok(await _workService.GetAsync<Link>(id));

    [HttpGet("google-map/{id}")]
    public async Task<IActionResult> GetGoogleMapAsync([FromRoute] Guid id) => Ok(await _workService.GetAsync<GoogleMap>(id));

    [HttpPost("google-map/save/{id}")]
    public async Task<IActionResult> SaveGoogleMapAsync([FromRoute] Guid id, [FromBody] GoogleMap args) => Ok(await _workService.SaveArgumentsAsync(id, args));

    [HttpGet("list-post-content")]
    public IActionResult GetListPostContent() => Ok(Enum.GetNames(typeof(PostContentType)).ToList());

    [HttpPost("sort")]
    public async Task<IActionResult> SortAsync([FromBody] List<Guid> workIds)
    {
        if (!workIds.Any()) return BadRequest("No work was found!");
        int i = 0;
        foreach (var workId in workIds)
        {
            var work = await _context.WorkItems.FirstOrDefaultAsync(x => x.WorkId == workId);
            if (work is null) return BadRequest($"Work {workId} not found!");
            work.SortOrder = i;
            i++;
        }
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpPost("child/sort")]
    public async Task<IActionResult> SortChildAsync([FromBody] List<Guid> workIds)
    {
        if (!workIds.Any()) return BadRequest("No work was found!");
        int i = 0;
        foreach (var workId in workIds)
        {
            var work = await _context.WorkContents.FirstOrDefaultAsync(x => x.Id == workId);
            if (work is null) return BadRequest($"Work {workId} not found!");
            work.SortOrder = i;
            i++;
        }
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }
}

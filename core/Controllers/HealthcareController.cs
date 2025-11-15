using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waffle.Core.Constants;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Healthcares;
using Waffle.Extensions;
using Waffle.Foundations;
using Waffle.Models.Args.Catalogs;
using Waffle.Models.Filters;

namespace Waffle.Controllers;

public class HealthcareController : BaseController
{
    private readonly ApplicationDbContext _context;
    private readonly ILogService _logger;
    public HealthcareController(ApplicationDbContext context, ILogService logService)
    {
        _context = context;
        _logger = logService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] HealthcareFilterOptions filterOptions)
    {
        try
        {
            var query = from a in _context.Catalogs
                        join b in _context.Healthcares on a.Id equals b.CatalogId
                        into ab
                        from b in ab.DefaultIfEmpty()
                        where a.Type == Entities.CatalogType.Hospital && a.Active
                        select new
                        {
                            a.Id,
                            a.Name,
                            a.Description,
                            a.ViewCount,
                            a.NormalizedName,
                            a.ModifiedDate,
                            a.CreatedDate,
                            b.Location,
                            a.Thumbnail,
                            minPoint = (from a1 in _context.Catalogs join a2 in _context.Healthcares on a1.Id equals a2.CatalogId where a1.ParentId == a.Id select a2.Point).Min(),
                            maxPoint = (from a1 in _context.Catalogs join a2 in _context.Healthcares on a1.Id equals a2.CatalogId where a1.ParentId == a.Id select a2.Point).Max()
                        };
            if (!string.IsNullOrEmpty(filterOptions.Name))
            {
                var name = SeoHelper.ToSeoFriendly(filterOptions.Name);
                query = query.Where(x => x.NormalizedName.Contains(name));
            }
            if (!string.IsNullOrEmpty(filterOptions.Location))
            {
                query = query.Where(x => x.Location == filterOptions.Location);
            }
            query = query.OrderByDescending(x => x.ModifiedDate);
            return Ok(await query.ToListAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("list-healthcare/{id}")]
    public async Task<IActionResult> ListHealthcareAsync([FromRoute] Guid id)
    {
        var query = from a in _context.Catalogs
                    join b in _context.Healthcares on a.Id equals b.CatalogId
                    where a.Type == CatalogType.Healthcare && a.ParentId == id
                    select new
                    {
                        a.Id,
                        a.Name,
                        a.Description,
                        a.ViewCount,
                        a.NormalizedName,
                        a.ModifiedDate,
                        a.CreatedDate,
                        b.Location,
                        a.Thumbnail,
                        b.Point,
                        catalogId = a.Id
                    };
        query = query.OrderBy(x => x.Point);
        return Ok(await query.ToListAsync());
    }

    [HttpGet("{catalogId}")]
    public async Task<IActionResult> GetContentAsync([FromRoute] Guid catalogId)
    {
        var catalog = await _context.Catalogs.FindAsync(catalogId);
        if (catalog is null)
        {
            return BadRequest("Data not found!");
        }
        var content = await _context.Healthcares.FirstOrDefaultAsync(x => x.CatalogId == catalog.Id);
        if (content is null)
        {
            content = new Healthcare
            {
                CatalogId = catalog.Id,
                Content = string.Empty,
                Location = string.Empty,
                Point = 0,
                Time = 0
            };
            await _context.Healthcares.AddAsync(content);
            await _context.SaveChangesAsync();
        }
        return Ok(new
        {
            content.Id,
            content.CatalogId,
            content.Location,
            catalog.Thumbnail,
            catalog.Name,
            content.Content,
            catalog.Description,
            catalog.CreatedDate,
            content.Point,
            catalog.ViewCount
        });
    }

    [HttpGet("hospital/{catalogId}")]
    public async Task<IActionResult> GetHospitalContentAsync([FromRoute] Guid catalogId)
    {
        var content = await _context.Healthcares.FirstOrDefaultAsync(x => x.CatalogId == catalogId);
        if (content is null)
        {
            return Ok(new Healthcare());
        }
        var catalog = await _context.Catalogs.FindAsync(catalogId);
        if (catalog is null)
        {
            return BadRequest("Data not found!");
        }
        return Ok(new
        {
            content.Id,
            content.CatalogId,
            content.Location,
            content.Point,
            catalog.Thumbnail,
            catalog.Name,
            content.Content,
            catalog.Description,
            catalog.CreatedDate,
            catalog.ViewCount
        });
    }

    [HttpPost("save")]
    public async Task<IActionResult> UpdateContentAsync([FromBody] Healthcare args)
    {
        var content = await _context.Healthcares.FirstOrDefaultAsync(x => x.CatalogId == args.CatalogId);
        if (content is null)
        {
            await _context.Healthcares.AddAsync(args);
            await _context.SaveChangesAsync();
            return Ok(IdentityResult.Success);
        }
        content.Location = args.Location;
        content.Point = args.Point;
        content.Content = args.Content;
        _context.Healthcares.Update(content);
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpPost("create-combo")]
    public async Task<IActionResult> CreateComboAsync([FromBody] ComboArgs args)
    {
        var user = await _context.Users.FindAsync(User.GetId());
        if (user is null) return BadRequest("User not found!");
        var catalog = new Catalog
        {
            Active = true,
            CreatedDate = DateTime.Now,
            CreatedBy = User.GetId(),
            Description = args.Description,
            Locale = Locale.VI_VN,
            ModifiedDate = DateTime.Now,
            Name = args.Name,
            NormalizedName = SeoHelper.ToSeoFriendly(args.Name),
            Type = CatalogType.Healthcare,
            Thumbnail = args.Thumbnail,
            ViewCount = 0,
            ParentId = args.ParentId
        };
        await _context.Catalogs.AddAsync(catalog);
        await _context.SaveChangesAsync();
        await _context.Healthcares.AddAsync(new Healthcare
        {
            CatalogId = catalog.Id,
            Content = args.Content,
            Location = args.Location,
            Point = args.Point
        });

        await _logger.AddAsync($"{user.Name} đã tạo gói khám {catalog.Name}");

        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpPost("combo/delete/{id}")]
    public async Task<IActionResult> DeleteComboAsync([FromRoute] Guid id)
    {
        var user = await _context.Users.FindAsync(User.GetId());
        if (user is null) return BadRequest("User not found!");
        var hc = await _context.Healthcares.FindAsync(id);
        if (hc == null) return BadRequest("Data not found!");
        _context.Healthcares.Remove(hc);
        var catalog = await _context.Catalogs.FindAsync(hc.CatalogId);
        if (catalog == null) return BadRequest("Data not found!");
        _context.Catalogs.Remove(catalog);

        await _logger.AddAsync($"{user.Name} đã xóa gói khám {catalog.Name}");
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("combo/update")]
    public async Task<IActionResult> UpdateComboAsync([FromBody] ComboArgs args)
    {
        var user = await _context.Users.FindAsync(User.GetId());
        if (user is null) return BadRequest("User not found!");

        var hc = await _context.Healthcares.FindAsync(args.Id);
        if (hc == null) return BadRequest("Data not found!");
        hc.Content = args.Content;
        hc.Point = args.Point;
        _context.Healthcares.Update(hc);

        var catalog = await _context.Catalogs.FindAsync(hc.CatalogId);
        if (catalog == null) return BadRequest("Data not found!");

        catalog.Name = args.Name;
        catalog.Description = args.Description;
        catalog.NormalizedName = SeoHelper.ToSeoFriendly(args.Name);
        catalog.Thumbnail = args.Thumbnail;
        catalog.ModifiedDate = DateTime.Now;
        _context.Catalogs.Update(catalog);

        await _logger.AddAsync($"{user.Name} đã tạo gói khám {catalog.Name}");

        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }


    [HttpGet("combo/{id}")]
    public async Task<IActionResult> GetCombo([FromRoute] Guid id)
    {
        var hc = await _context.Healthcares.FindAsync(id);
        if (hc == null) return BadRequest("Data not found!");

        var catalog = await _context.Catalogs.FindAsync(hc.CatalogId);
        if (catalog == null) return BadRequest("Data not found!");

        return Ok(new
        {
            hc.Id,
            hc.Content,
            hc.Point,
            catalog.Name,
            catalog.Description,
            catalog.Thumbnail
        });
    }
}

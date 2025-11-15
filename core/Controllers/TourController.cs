using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Tours;
using Waffle.Foundations;
using Waffle.Models;
using Waffle.Models.Filters;
using Waffle.Models.ViewModels;

namespace Waffle.Controllers;

public class TourController : BaseController
{
    private readonly ApplicationDbContext _context;
    private readonly ILogService _logService;
    public TourController(ApplicationDbContext context, ILogService logService)
    {
        _context = context;
        _logService = logService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] TourFilterOptions filterOptions)
    {
        var query = from a in _context.Catalogs
                    join b in _context.TourCatalogs on a.Id equals b.CatalogId
                    where a.Type == filterOptions.Type && a.Active
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
                        Point = _context.Catalogs.Where(x => x.ParentId == a.Id).Join(_context.TourCatalogs, c => c.Id, t => t.CatalogId, (c, t) => t.Point).FirstOrDefault(),
                        b.Amenities,
                        b.Tags,
                        a.ParentId,
                        SelfPoint = b.Point
                    };
        if (filterOptions.ParentId != null)
        {
            query = query.Where(x => x.ParentId == filterOptions.ParentId);
        }
        if (filterOptions.ParentId == null)
        {
            query = query.Where(x => x.ParentId == null);
        }
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

    [HttpGet("{catalogId}")]
    public async Task<IActionResult> GetContentAsync([FromRoute] Guid catalogId)
    {
        var catalog = await _context.Catalogs.FindAsync(catalogId);
        if (catalog is null) return BadRequest("Data not found!");
        var content = await _context.TourCatalogs.FirstOrDefaultAsync(x => x.CatalogId == catalogId);
        if (content is null)
        {
            return Ok(new TourViewModel
            {
                CatalogId = catalogId,
                Thumbnail = catalog.Thumbnail,
                ViewCount = catalog.ViewCount,
                Name = catalog.Name,
                Type = catalog.Type
            });
        }
        var amenities = from a in _context.Amenities
                        join b in _context.TourAmenities on a.Id equals b.AmenityId
                        where b.CatalogId == catalogId
                        select a;
        return Ok(new TourViewModel
        {
            Id = content.Id,
            CatalogId = content.CatalogId,
            Location = content.Location,
            Amenities = await amenities.ToListAsync(),
            Tags = content.Tags?.Split(",").ToList() ?? new(),
            Images = content.Images?.Split(",").ToList() ?? new(),
            Point = content.Point,
            Thumbnail = catalog.Thumbnail,
            Itineraries = await _context.Itineraries.Where(x => x.CatalogId == catalogId).ToListAsync(),
            Name = catalog.Name,
            Content = content.Content,
            ViewCount = catalog.ViewCount,
            Type = catalog.Type,
            ParentId = catalog.ParentId
        });
    }

    [HttpPost("save")]
    public async Task<IActionResult> UpdateContentAsync([FromBody] TourCatalog args)
    {
        try
        {
            var content = await _context.TourCatalogs.FirstOrDefaultAsync(x => x.CatalogId == args.CatalogId);

            var currentAmenities = await _context.TourAmenities.Where(x => x.CatalogId == args.CatalogId).ToListAsync();
            if (currentAmenities.Any())
            {
                _context.TourAmenities.RemoveRange(currentAmenities);
            }
            if (!string.IsNullOrWhiteSpace(args.Amenities))
            {
                var amenities = args.Amenities.Split(",").Select(x => Guid.Parse(x));
                if (amenities.Any())
                {
                    foreach (var item in amenities)
                    {
                        await _context.TourAmenities.AddAsync(new TourAmenity
                        {
                            AmenityId = item,
                            CatalogId = args.CatalogId ?? Guid.Empty
                        });
                    }
                }
            }

            if (content is null)
            {
                await _context.TourCatalogs.AddAsync(args);
                await _context.SaveChangesAsync();
                return Ok(IdentityResult.Success);
            }
            content.Location = args.Location;
            content.Point = args.Point;
            content.Amenities = args.Amenities;
            content.Tags = args.Tags;
            content.Images = args.Images;
            content.Content = args.Content;
            _context.TourCatalogs.Update(content);
            await _context.SaveChangesAsync();
            return Ok(IdentityResult.Success);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("itinerary/{id}")]
    public async Task<IActionResult> ItineraryListAsync([FromRoute] Guid id)
    {
        var query = _context.Itineraries.Where(x => x.CatalogId == id);
        return Ok(new
        {
            data = await query.ToListAsync(),
            total = await query.CountAsync()
        });
    }

    [HttpPost("itinerary/add")]
    public async Task<IActionResult> AddItineraryAsync([FromBody] Itinerary args)
    {
        var catalog = await _context.Catalogs.FindAsync(args.CatalogId);
        if (catalog is null)
        {
            return BadRequest("Data not found!");
        }
        await _context.Itineraries.AddAsync(args);
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpPost("itinerary/update")]
    public async Task<IActionResult> ItineraryUpdateAsync([FromBody] Itinerary args)
    {
        var itinerary = await _context.Itineraries.FindAsync(args.Id);
        if (itinerary is null)
        {
            return BadRequest("Data not found!");
        }
        itinerary.Title = args.Title;
        itinerary.Content = args.Content;
        _context.Itineraries.Update(itinerary);
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpPost("itinerary/delete/{id}")]
    public async Task<IActionResult> ItineraryDeleteAsync([FromRoute] Guid id)
    {
        var itinerary = await _context.Itineraries.FindAsync(id);
        if (itinerary is null)
        {
            return BadRequest("Data not found!");
        }
        _context.Itineraries.Remove(itinerary);
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpGet("amenities")]
    public async Task<IActionResult> AmenitiesAsync()
    {
        var query = await _context.Amenities
            .Select(x => new
            {
                value = x.Id,
                label = x.Name
            })
            .ToListAsync();
        return Ok(query);
    }

    [HttpGet("cities")]
    public async Task<IActionResult> GetCitiesAsync()
    {
        return Ok(await _context.Cities.Select(x => new
        {
            value = x.Name,
            label = x.Name
        }).ToListAsync());
    }

    [HttpGet("list-city")]
    public async Task<IActionResult> GetListCityAsync([FromQuery] BasicFilterOptions filterOptions)
    {
        var query = from a in _context.Cities
                    select new
                    {
                        a.Name,
                        a.Id
                    };
        query = query.OrderBy(x => x.Name);
        return Ok(await ListResult<object>.Success(query, filterOptions));
    }

    [HttpPost("add-city")]
    public async Task<IActionResult> AddCityAsync([FromBody] City args)
    {
        if (string.IsNullOrWhiteSpace(args.Name)) return BadRequest("City name is required!");
        if (await _context.Cities.AnyAsync(x => x.Name.ToLower() == args.Name.ToLower())) return BadRequest("Địa điểm đã tồn tại!");
        await _context.Cities.AddAsync(args);
        await _logService.AddAsync($"Thêm địa điểm {args.Name}");
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("delele-city/{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city == null) return BadRequest("City not found!");
        _context.Cities.Remove(city);
        await _logService.AddAsync($"Xóa địa điểm {city.Name}");
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("save-point")]
    public async Task<IActionResult> SavePointAsync([FromBody] TourCatalog args)
    {
        var tour = await _context.TourCatalogs.FirstOrDefaultAsync(x => x.CatalogId == args.CatalogId);
        if (tour is null)
        {
            tour = new TourCatalog
            {
                CatalogId = args.CatalogId,
                Point = args.Point
            };
            await _context.TourCatalogs.AddAsync(tour);
        }
        else
        {
            tour.Point = args.Point;
            _context.TourCatalogs.Update(tour);
        }
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }
}

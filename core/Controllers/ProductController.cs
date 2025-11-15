using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using Waffle.Core.Constants;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Ecommerces;
using Waffle.Entities.Healthcares;
using Waffle.Entities.Tours;
using Waffle.Extensions;
using Waffle.ExternalAPI;
using Waffle.Foundations;
using Waffle.Models;
using Waffle.Models.Args;
using Waffle.Models.Filters;
using Waffle.Models.Params.Products;
using Waffle.Models.ViewModels.Products;

namespace Waffle.Controllers;

public class ProductController : BaseController
{
    private readonly ICatalogService _catalogService;
    private readonly IWorkService _workService;
    private readonly IProductService _productService;
    private readonly ILogService _appLogService;
    private readonly ApplicationDbContext _context;

    public ProductController(ICatalogService catalogService, IWorkService workService, IProductService productService, ILogService appLogService, ApplicationDbContext context)
    {
        _catalogService = catalogService;
        _workService = workService;
        _productService = productService;
        _appLogService = appLogService;
        _context = context;
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] HealthcareFilterOptions filterOptions)
    {
        var query = from a in _context.Catalogs
                    join b in _context.Products on a.Id equals b.CatalogId
                    where a.Active
                    select new ProductListItem
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Description = a.Description,
                        ViewCount = a.ViewCount,
                        NormalizedName = a.NormalizedName,
                        ModifiedDate = a.ModifiedDate,
                        CreatedDate = a.CreatedDate,
                        Point = b.Point,
                        Thumbnail = a.Thumbnail,
                        SKU = b.SKU,
                        Galleries = b.Galleries,
                        Summary = b.Summary
                    };
        if (!string.IsNullOrEmpty(filterOptions.Name))
        {
            var name = SeoHelper.ToSeoFriendly(filterOptions.Name);
            query = query.Where(x => x.NormalizedName.Contains(name));
        }
        query = query.OrderByDescending(x => x.ModifiedDate);
        return Ok(await query.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContentAsync([FromRoute] Guid id)
    {
        var content = await _context.Products.FirstOrDefaultAsync(x => x.CatalogId == id);
        if (content is null)
        {
            return Ok(new Healthcare());
        }
        var catalog = await _context.Catalogs.FindAsync(id);
        if (catalog is null)
        {
            return BadRequest("Data not found!");
        }
        return Ok(new
        {
            content.Id,
            content.CatalogId,
            content.Point,
            catalog.Thumbnail,
            catalog.Name,
            content.Content,
            content.Brand,
            content.SKU,
            catalog.Description,
            galleries = content.Galleries?.Split(","),
            content.Summary,
            content.Cert1Name,
            content.Cert2Name,
            content.Cert1File,
            content.Cert2File
        });
    }

    [HttpGet("count")]
    public async Task<IActionResult> CountAsync() => Ok(await _productService.CountAsync());

    [HttpPost("save")]
    public async Task<IActionResult> SaveAsync([FromBody] Product args)
    {
        var catalog  = await _catalogService.FindAsync(args.CatalogId);
        if (catalog is null) return BadRequest("Catalog not found!");
        if (args.Point < 0) return BadRequest("Price is not valid!");
        var user = await _context.Users.FindAsync(User.GetId());
        if (user is null) return BadRequest("User not found!");
        await _appLogService.AddAsync($"{user.UserName} đã cập nhật sản phẩm: {catalog.Name}");
        return Ok(await _productService.SaveAsync(args));
    }

    [HttpGet("image/{id}")]
    public async Task<IActionResult> GetProductImageAsync([FromRoute] Guid id) => Ok(await _catalogService.GetProductImageAsync(id));

    [HttpPost("image/save")]
    public async Task<IActionResult> AddImageAsync([FromBody] SaveImageModel args)
    {
        return Ok(await _workService.SaveProductImageAsync(args));
    }

    [HttpPost("cart-items/{type}"), AllowAnonymous]
    public async Task<IActionResult> GetCartItemsAsync([FromRoute] string type, [FromBody] List<CartItem> args)
    {
        if (args is null || !args.Any())
        {
            return View(PartialViewName.Empty, new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
        foreach (var item in args)
        {
            item.Catalog = await _catalogService.FindAsync(item.ProductId);
            item.Product = await _productService.GetByCatalogIdAsync(item.ProductId);
        }
        if ("checkout".Equals(type))
        {
            return View("/Pages/Products/Checkout/_Products.cshtml", args);
        }
        return View("/Pages/Products/Cart/_Products.cshtml", args);
    }

    [HttpPost("brand/save")]
    public async Task<IActionResult> SaveBrandAsync([FromBody] SaveBrandModel args) => Ok(await _productService.SaveBrandAsync(args));

    [HttpPost("buy")]
    public async Task<IActionResult> BuyAsync([FromBody] BuyProduct args)
    {
        var catalog = await _context.Catalogs.FindAsync(args.CatalogId);
        if (catalog == null) return BadRequest("Catalog not found!");
        var product = await _context.Products.FindAsync(args.ProductId);
        if (product == null) return BadRequest("Product not found!");
        var user = await _context.Users.FindAsync(User.GetId());
        if (user is null)
        {
            return BadRequest("User not found!");
        }
        var form = new Form
        {
            CreatedDate = DateTime.Now,
            Point = product.Point,
            CatalogId = catalog.Id,
            Status = FormStatus.New,
            UserId = User.GetId()
        };
        await _context.Forms.AddAsync(form);
        await _context.SaveChangesAsync();
        
        return Ok(IdentityResult.Success);
    }
}

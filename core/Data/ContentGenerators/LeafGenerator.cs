using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Waffle.Core.Foundations;
using Waffle.Entities;
using Waffle.Models.Components;

namespace Waffle.Data.ContentGenerators;

public class LeafGenerator : BaseGenerator
{
    public LeafGenerator(ApplicationDbContext context) : base(context)
    {
        
    }

    public async Task EnsurePricingAsync()
    {
        var pricing = await _context.Catalogs.FirstOrDefaultAsync();
        if (pricing is null)
        {
            
        }
    }

    public async Task EnsureThankToSubscribleAsync()
    {
        var normalizedName = "thank-to-subscribe";
        var catalog = await _context.Catalogs.FirstOrDefaultAsync(x => x.NormalizedName == normalizedName);
        if (catalog is null)
        {
            catalog = new Catalog
            {
                Active = true,
                NormalizedName = normalizedName,
                Name = "Thank to subscribe",
                Type = CatalogType.Default
            };

            await _context.Catalogs.AddAsync(catalog);
            await _context.SaveChangesAsync();
        }
    }

    private async Task EnsureHomeAsync()
    {
        var home = await _context.Catalogs.FirstOrDefaultAsync(x => x.NormalizedName == "/index");
        if (home is null)
        {
            home = new Catalog
            {
                NormalizedName = "/index",
                Active = true,
                CreatedDate = DateTime.Now,
                Type = CatalogType.Entry,
            };
            await _context.Catalogs.AddAsync(home);
            await _context.SaveChangesAsync();
        }
        var components = from a in _context.WorkItems
                         join b in _context.WorkContents on a.WorkId equals b.Id
                         where a.CatalogId == home.Id
                         select b;
        var sponsorComponent = await _context.Components.FirstOrDefaultAsync(x => x.NormalizedName == "Sponsor");
        if (sponsorComponent is null)
        {
            sponsorComponent = new Component
            {
                Active = true,
                NormalizedName = "Sponsor"
            };
            await _context.Components.AddAsync(sponsorComponent);
            await _context.SaveChangesAsync();
        }

        if (!await components.AnyAsync(x => x.Id == sponsorComponent.Id))
        {
            var sponsorBrands = new List<SponsorBrand>();
            for (int i = 0; i < 10;  i++)
            {
                sponsorBrands.Add(new SponsorBrand
                {
                    Id = Guid.NewGuid(),
                    Logo = "https://placehold.jp/150x100.png",
                    Name = "Brand Name",
                    Url = "#"
                });
            }
            var sponsor = new Sponsor
            {
                Brands = sponsorBrands
            };
            var work = new Entities.WorkContent
            {
                Active = true,
                ComponentId = sponsorComponent.Id,
                Name = sponsorComponent.Name,
                Arguments = JsonSerializer.Serialize(sponsor)
            };
            await _context.WorkContents.AddAsync(work);
            await _context.SaveChangesAsync();
            await _context.WorkItems.AddAsync(new Entities.WorkItem
            {
                CatalogId = home.Id,
                WorkId = work.Id,
                SortOrder = components.Count() + 1
            });
            await _context.SaveChangesAsync();
        }
    }

    public override async Task RunAsync()
    {
        //await EnsureHomeAsync();
        await EnsurePricingAsync();
        //await EnsureThankToSubscribleAsync();
    }
}

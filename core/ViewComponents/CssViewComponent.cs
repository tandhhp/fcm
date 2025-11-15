using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waffle.Data;
using Waffle.Models.Components;

namespace Waffle.ViewComponents
{
    public class CssViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public CssViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid? catalogId)
        {
            if (catalogId != null)
            {
                var css = await _context.Components.FirstOrDefaultAsync(x => x.NormalizedName.Equals(nameof(Css)));
                if (css != null)
                {
                    var query = from a in _context.WorkItems
                                join b in _context.WorkContents on a.WorkId equals b.Id
                                where a.CatalogId == catalogId && b.Active && b.ComponentId == css.Id
                                select b.Id;
                    ViewBag.CSS = await query.ToListAsync();
                }
            }
            return View();
        }
    }
}

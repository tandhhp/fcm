using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Pages.Users;

public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public IndexModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public ApplicationUser? ApplicationUser;
    public ListResult<Catalog>? Articles;
    public List<Achievement> Achievements = new();
    public List<Catalog> Forms = new();

    public async Task<IActionResult> OnGetAsync(Guid? id, Guid? achievementId)
    {
        if (id == null) return NotFound();
        ApplicationUser = await _userManager.FindByIdAsync(id.GetValueOrDefault().ToString());
        if (ApplicationUser is null) return NotFound();

        Achievements = await _context.Achievements.Where(x => x.IsApproved).Where(x => x.UserId == id).ToListAsync();
        if (Achievements.Any())
        {
            var ach = Achievements.FirstOrDefault(x => x.Id == achievementId);
            if (ach != null)
            {
                ViewData["Image"] = ach.Icon;
            }
            else
            {
                ViewData["Image"] = Achievements.First().Icon;
            }
        }

        Forms = await _context.Forms.Where(x => x.UserId == id).Join(_context.Catalogs, p =>
        p.CatalogId, c => c.Id, (p, c) => c).ToListAsync();

        return Page();
    }
}

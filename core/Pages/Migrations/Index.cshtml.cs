using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Models.Components;

namespace Waffle.Pages.Migrations
{
    public class IndexModel : PageModel
    {
        private readonly IMigrationService _migrationService;
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context, IMigrationService migrationService)
        {
            _context = context;
            _migrationService = migrationService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _migrationService.MoveComponentsAsync(nameof(Editor), "BlockEditor");
            return Page();
        }
    }
}

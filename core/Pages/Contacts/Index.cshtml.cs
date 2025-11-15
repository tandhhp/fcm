using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using Waffle.Core.Foundations;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities.Contacts;
using Waffle.ExternalAPI.SendGrids;

namespace Waffle.Pages.Contacts
{
    public class IndexModel<TUser> : EntryPageModel where TUser : class
    {
        private readonly UserManager<TUser> _userManager;
        private readonly ISettingService _appSettingService;
        private readonly ApplicationDbContext _context;

        public IndexModel(ICatalogService catalogService, UserManager<TUser> userManager, ISettingService appSettingService, ApplicationDbContext context) : base(catalogService)
        {
            _userManager = userManager;
            _appSettingService = appSettingService;
            _context = context;
        }

        [BindProperty]
        public Contact Input { get; set; } = default!;

        public IdentityResult IdentityResult { get; set; } = IdentityResult.Success;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _context.Contacts.AddAsync(new Contact
            {
                Email = Input.Email,
                PhoneNumber = Input.PhoneNumber,
                CreatedDate = DateTime.Now,
                Name = Input.Name,
                Note = Input.Note
            });
            await _context.SaveChangesAsync();
            return Redirect("/contacts/thank");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;

namespace Waffle.Pages.Contacts
{
    public class ThankModel : EntryPageModel
    {
        public ThankModel(ICatalogService catalogService) : base(catalogService)
        {
        }

        public void OnGet()
        {
        }
    }
}

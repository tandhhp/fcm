using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.ExternalAPI.Interfaces;
using Waffle.ExternalAPI.Models;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.ViewComponents
{
    public class FacebookProductViewComponent : ViewComponent
    {
        private readonly IFacebookService _facebookService;
        private readonly ISettingService _appService;
        public FacebookProductViewComponent(IFacebookService facebookService, ISettingService appSettingService)
        {
            _facebookService = facebookService;
            _appService = appSettingService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid workId)
        {
            var app = await _appService.GetAsync<Facebook>(nameof(Facebook));
            if (app is null)
            {
                return View(Empty.DefaultView, new ErrorViewModel
                {
                    RequestId = workId.ToString()
                });
            }
            return View(await _facebookService.GetProductsAsync("243943267914049", app.PageAccessToken, 4));
        }
    }
}

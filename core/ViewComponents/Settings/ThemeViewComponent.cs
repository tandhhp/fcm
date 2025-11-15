using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services;
using Waffle.Models.Components;
using Waffle.Models;
using Waffle.Models.Settings;

namespace Waffle.ViewComponents.Settings;

public class ThemeViewComponent : ViewComponent
{
    private readonly ISettingService _settingService;

    public ThemeViewComponent(ISettingService settingService)
    {
        _settingService = settingService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var theme = await _settingService.GetAsync<Theme>(nameof(Theme));
        theme ??= new Theme
            {
                Name = "default"
            };
        return View(theme);
    }
}

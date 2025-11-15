using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Options;
using Waffle.Models.Settings;

namespace Waffle.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    private readonly ISettingService _settingService;
    private readonly SettingOptions Options;
    public FooterViewComponent(ISettingService settingService, IOptions<SettingOptions> options)
    {
        _settingService = settingService;
        Options = options.Value;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var footer = await _settingService.GetAsync<Footer>(nameof(Footer));
        footer ??= new Footer();
        return View(Options.Theme, footer);
    }
}

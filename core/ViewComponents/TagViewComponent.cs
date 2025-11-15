using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;

namespace Waffle.ViewComponents;

public class TagViewComponent : ViewComponent
{
    private readonly IWorkService _workService;
    private readonly ICatalogService _catalogService;
    public TagViewComponent(IWorkService workService, ICatalogService catalogService)
    {
        _workService = workService;
        _catalogService = catalogService;

    }

    public async Task<IViewComponentResult> InvokeAsync(Guid workId)
    {
        return View(await _catalogService.ListRandomTagAsync());
    }
}

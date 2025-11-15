using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class LookbookViewComponent : ViewComponent
{
    private readonly IWorkService _workService;
    public LookbookViewComponent(IWorkService workService)
    {
        _workService = workService;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid workId)
    {
        var lookbook = await _workService.FindAsync(workId);
        if (lookbook is not null)
        {
            ViewBag.Images = GetImagesAsync(workId);
        }
        return View();
    }

    private async IAsyncEnumerable<Image?> GetImagesAsync(Guid workId)
    {
        var images = await _workService.GetWorkContentChildsAsync(workId);
        foreach (var image in images)
        {
            if (string.IsNullOrEmpty(image.Arguments))
            {
                continue;
            }
            yield return _workService.Get<Image>(image.Arguments);
        }
    }
}

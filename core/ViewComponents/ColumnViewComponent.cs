using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class ColumnViewComponent : ViewComponent
{
    private readonly IWorkService _workService;
    public ColumnViewComponent(IWorkService workService)
    {
        _workService = workService;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid workId)
    {
        var column = await _workService.GetAsync<Column>(workId);
        if (column is null)
        {
            return View(Empty.DefaultView, new ErrorViewModel
            {
                RequestId = workId.ToString()
            });
        }
        column.Items = await _workService.GetComponentsInColumnAsync(workId);
        return View(column);
    }
}

using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class RowViewComponent : ViewComponent
{
    private readonly IWorkService _workService;
    public RowViewComponent(IWorkService workService)
    {
        _workService = workService;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid id)
    {
        var row = await _workService.GetAsync<Row>(id);
        row ??= new Row();
        row.Columns = await _workService.ListChildIdAsync(id);
        return View(row);
    }
}

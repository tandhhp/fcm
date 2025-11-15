using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.ViewComponents
{
    public class BlockEditorViewComponent : ViewComponent
    {
        private readonly IWorkService _workService;
        public BlockEditorViewComponent(IWorkService workService)
        {
            _workService = workService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid id)
        {
            var blockEditor = await _workService.GetAsync<List<BlockEditorBlock>>(id);
            if (blockEditor is null)
            {
                return View(Empty.DefaultView, new ErrorViewModel
                {
                    RequestId = id.ToString()
                });
            }
            return View(blockEditor);
        }
    }
}
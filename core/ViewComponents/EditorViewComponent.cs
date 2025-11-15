using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class EditorViewComponent : BaseViewComponent<Editor>
{
    public EditorViewComponent(IWorkService workService) : base(workService)
    {
    }
}

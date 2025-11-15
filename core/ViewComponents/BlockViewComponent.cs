using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Models.Components;

namespace Waffle.ViewComponents
{
    public class BlockViewComponent : BaseViewComponent<BlockEditor>
    {
        public BlockViewComponent(IWorkService workService) : base(workService)
        {
        }
    }
}

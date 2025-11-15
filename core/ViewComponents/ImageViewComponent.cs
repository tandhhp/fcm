using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Models.Components;

namespace Waffle.ViewComponents
{
    public class ImageViewComponent : BaseViewComponent<Image>
    {
        public ImageViewComponent(IWorkService workService) : base(workService) { }
    }
}

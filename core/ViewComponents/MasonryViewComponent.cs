using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class MasonryViewComponent : BaseViewComponent<Masonry>
{
    public MasonryViewComponent(IWorkService workService) : base(workService) { }
}

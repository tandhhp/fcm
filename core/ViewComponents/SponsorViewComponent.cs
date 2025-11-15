using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class SponsorViewComponent : BaseViewComponent<Sponsor>
{
    public SponsorViewComponent(IWorkService workService) : base(workService)
    {
    }
}

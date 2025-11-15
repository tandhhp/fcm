using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class TestimonialViewComponent : BaseViewComponent<Testimonial>
{
    public TestimonialViewComponent(IWorkService workService) : base(workService)
    {
    }
}

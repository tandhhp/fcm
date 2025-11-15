using Waffle.Models.Components;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;

namespace Waffle.ViewComponents;

public class ListGroupViewComponent : BaseViewComponent<ListGroup>
{
    public ListGroupViewComponent(IWorkService workService) : base(workService)
    {
    }
}

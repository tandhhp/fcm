using Waffle.Models.Components;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Foundations;
using Waffle.Core.Options;
using Microsoft.Extensions.Options;

namespace Waffle.ViewComponents;

public class JumbotronViewComponent : BaseViewComponent<Jumbotron>
{
    private readonly SettingOptions Options;
    public JumbotronViewComponent(IWorkService workService, IOptions<SettingOptions> options) : base(workService) {
        Options = options.Value;
    }

    protected override Jumbotron Extend(Jumbotron work)
    {
        ViewName = Options.Theme;
        return base.Extend(work);
    }
}

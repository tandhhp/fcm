using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class ContactFormViewComponent : BaseViewComponent<ContactForm>
{
    public ContactFormViewComponent(IWorkService workService) : base(workService) { }

    protected override Task<ContactForm> ExtendAsync(ContactForm work)
    {
        ViewName = work.Type;
        return base.ExtendAsync(work);
    }
}

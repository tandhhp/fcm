using Waffle.Entities.Contacts;

namespace Waffle.Core.Services.Contacts.Models;

public class SubmitFormModel : Contact
{
    public Guid WorkId { get; set; }
}

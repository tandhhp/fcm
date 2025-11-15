using Waffle.Entities;

namespace Waffle.Core.Services.Contacts.Args;

public class ContactBookArgs : BaseEntity
{
    public Guid EventId { get; set; }
    public DateTime EventDate { get; set; }
    public string? Note { get; set; }
}

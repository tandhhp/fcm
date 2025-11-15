using System.ComponentModel.DataAnnotations.Schema;
using Waffle.Entities.Contacts;

namespace Waffle.Entities;

public class ContactActivity : BaseEntity
{
    public DateTime CalledDate { get; set; }
    public string? Note { get; set; }

    [ForeignKey(nameof(Contact))]
    public Guid ContactId { get; set; }

    public Contact? Contact { get; set; }
}

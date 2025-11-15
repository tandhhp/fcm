using Waffle.Entities.Contacts;

namespace Waffle.Core.Services.Contacts.Models;

public class ContactCreateArgs
{
    public string PhoneNumber { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Email { get; set; }
    public Guid? UserId { get; set; }
    public int? DistrictId { get; set; }
    public int? JobKindId { get; set; }
    public MarriedStatus? MarriedStatus { get; set; }
    public string? Note { get; set; }
    public bool? Gender { get; set; }
    public int? TransportId { get; set; }
}

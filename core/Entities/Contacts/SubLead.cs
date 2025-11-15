using System.ComponentModel.DataAnnotations;

namespace Waffle.Entities.Contacts;

public class SubLead
{
    [Key]
    public Guid Id { get; set; }
    public Guid LeadId { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? Gender { get; set; }
    public string? IdentityNumber { get; set; }
    public string? Address { get; set; }

    public Lead? Lead { get; set; }
}

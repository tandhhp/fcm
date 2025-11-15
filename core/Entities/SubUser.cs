namespace Waffle.Entities;

public class SubUser : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? IdentityNumber { get; set; }
    public bool? Gender { get; set; }
}

namespace Waffle.Core.Services.Users.Args;

public class UpdateTelesaleArgs
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public int BranchId { get; set; }
    public int? TeamId { get; set; }
    public string? LineCode { get; set; }
    public Guid? ManagerId { get; set; }
}

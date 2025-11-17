using Waffle.Entities;

namespace Waffle.Models;

public class LoginModel
{
    public string? UserName { get; set; }
    public string Password { get; set; } = default!;
    public bool IsAdmin { get; set; }
}

public class ChangePasswordModel : BaseEntity
{
    public string CurrentPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}

public class ExportDateFilterOptions
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
public class CreateUserModel : LoginModel
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Avatar { get; set; }
    public string? Address { get; set; }
    public string Name { get; set; } = default!;
    public string? IdentityNumber { get; set; }
    public string? ContractCode { get; set; }
    public string Role { get; set; } = default!;
    public Guid? DosId { get; set; }
    public Guid? SmId { get; set; }
    public int BranchId { get; set; }
    public DateOnly? ContractDate { get; set; }
    public Guid? TmId { get; set; }
    public int? SourceId { get; set; }
    public string? Position { get; set; }
    public Guid? DotId { get; set; }
    public string? LineCode { get; set; }
}

public class AddToRoleModel : BaseEntity
{
    public string RoleName { get; set; } = default!;
}

public class RemoveFromRoleModel : BaseEntity
{
    public string RoleName { get; set; } = default!;
}

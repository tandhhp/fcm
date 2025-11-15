namespace Waffle.Core.Services.Users.Args;

public class AddClaimArgs
{
    public string UserId { get; set; } = default!;
    public string ClaimType { get; set; } = default!;
    public string ClaimValue { get; set; } = default!;
}

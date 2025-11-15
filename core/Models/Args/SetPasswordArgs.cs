namespace Waffle.Models.Args;

public class SetPasswordArgs
{
    public string UserId { get; set; } = default!;
    public string? Password { get; set; }
}

namespace Waffle.Core.Services.Users.Args;

public class UserImportArgs
{
    public IFormFile? File { get; set; }
    public int BranchId { get; set; }
}

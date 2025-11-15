namespace Waffle.Core.Services.Departments.Models;

public class CreateDepartmentArgs
{
    public int BranchId { get; set; }
    public string Name { get; set; } = default!;
    public Guid? LeaderId { get; set; }
}

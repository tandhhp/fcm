namespace Waffle.Core.Services.Teams.Models;

public class CreateTeamArgs
{
    public int DepartmentId { get; set; }
    public string Name { get; set; } = default!;
    public Guid? LeaderId { get; set; }
    public int? CallCenterId { get; set; }
    public int? GroupDataId { get; set; }
}

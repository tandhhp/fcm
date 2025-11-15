using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Waffle.Entities.Contacts;

namespace Waffle.Entities.Users;

public class Team : BaseEntity<int>
{
    public Guid? LeaderId { get; set; }
    [StringLength(256)]
    public string Name { get; set; } = default!;
    [ForeignKey(nameof(Department))]
    public int DepartmentId { get; set; }
    [ForeignKey(nameof(CallCenter))]
    public int? CallCenterId { get; set; }
    [ForeignKey(nameof(GroupData))]
    public int? GroupDataId { get; set; }

    public Department? Department { get; set; }
    public CallCenter? CallCenter { get; set; }
    public GroupData? GroupData { get; set; }
    public ICollection<ApplicationUser>? Users { get; set; }
}

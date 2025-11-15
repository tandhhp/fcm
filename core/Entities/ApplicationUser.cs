using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Waffle.Entities.Users;

namespace Waffle.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    [StringLength(256)]
    public string Name { get; set; } = default!;
    [StringLength(2048)]
    public string? Address { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public bool? Gender { get; set; }
    public int Loyalty { get; set; }
    [StringLength(2048)]
    public string? Avatar { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    [StringLength(12)]
    public string? IdentityNumber { get; set; }
    public string? ContractCode { get; set; }
    public int Token { get; set; }
    public Guid? SmId { get; set; }
    public Guid? DosId { get; set; }
    public int? LoanPoint { get; set; }
    [Column(TypeName = "money")]
    public decimal Amount { get; set; }
    [ForeignKey(nameof(Branch))]
    public int BranchId { get; set; }
    public DateOnly? ContractDate { get; set; }
    public Guid? TmId { get; set; }
    public UserStatus Status { get; set; }
    public Guid? DotId { get; set; }
    [ForeignKey(nameof(Team))]
    public int? TeamId { get; set; }
    [ForeignKey(nameof(District))]
    public int? DistrictId { get; set; }
    public Guid? ManagerId { get; set; }
    [ForeignKey(nameof(Source))]
    public int? SourceId { get; set; }
    [StringLength(2048)]
    public string? LineCode { get; set; }
    [StringLength(512)]
    [Comment("Chức vụ")]
    public string? Position { get; set; }

    public virtual Branch? Branch { get; set; }
    public virtual District? District { get; set; }
    public virtual Team? Team { get; set; }
    public virtual Source? Source { get; set; }
}

public enum UserStatus
{
    Working,
    Leave
}

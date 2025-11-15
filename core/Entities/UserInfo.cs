using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Waffle.Entities;

public class UserInfo : BaseEntity
{
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    [Comment("Tiền sử sức khỏe")]
    public string? HealthHistory { get; set; }
    [Comment("Đặc điểm gia đình")]
    public string? FamilyCharacteristics { get; set; }
    [Comment("Đặc điểm tính cách")]
    public string? Personality { get; set; }
    [Comment("Mối quan tâm")]
    public string? Concerns { get; set; }

    [JsonIgnore]
    public ApplicationUser? User { get; set; }
}

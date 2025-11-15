using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Waffle.Entities;

public class FileContent : BaseEntity
{
    [JsonPropertyName("name")]
    [StringLength(256)]
    public string Name { get; set; } = default!;
    [JsonPropertyName("size")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Size { get; set; }
    [JsonPropertyName("type")]
    [StringLength(128)]
    public string Type { get; set; } = default!;
    [JsonPropertyName("url")]
    [StringLength(2048)]
    public string Url { get; set; } = default!;
    public DateTime UploadDate { get; set; }
    public Guid UploadBy { get; set; }
}

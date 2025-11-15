using System.Text.Json.Serialization;

namespace Waffle.Entities;

public abstract class BasicWorkContent : BaseEntity
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("componentId")]
    public Guid ComponentId { get; set; }
    [JsonPropertyName("parentId")]
    public Guid? ParentId { get; set; }
    [JsonPropertyName("active")]
    public bool Active { get; set; }
    [JsonPropertyName("sortOrder")]
    public int SortOrder { get; set; }
}

public class WorkContent : BasicWorkContent
{
    [JsonPropertyName("arguments")]
    public string? Arguments { get; set; }
}

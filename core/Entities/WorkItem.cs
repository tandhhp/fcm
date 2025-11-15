using System.Text.Json.Serialization;

namespace Waffle.Entities
{
    public class WorkItem
    {
        [JsonPropertyName("workId")]
        public Guid WorkId { get; set; }
        [JsonPropertyName("catalogId")]
        public Guid CatalogId { get; set; }
        [JsonPropertyName("sortOrder")]
        public int SortOrder { get; set; }
    }
}

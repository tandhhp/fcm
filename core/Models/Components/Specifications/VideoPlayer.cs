using System.Text.Json.Serialization;

namespace Waffle.Models.Components.Specifications
{
    public class VideoPlayer
    {
        [JsonPropertyName("format")]
        public VideoPlayerFormat Format { get; set; }
        [JsonPropertyName("embedId")]
        public string? EmbedId { get; set; }

        [JsonIgnore]
        public string? Description { get; set; }
    }

    public enum VideoPlayerFormat
    {
        HTML,
        YouTube,
        Vimeo
    }
}

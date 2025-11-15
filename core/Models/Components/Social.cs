using System.Text.Json.Serialization;

namespace Waffle.Models.Components;

public class Social
{
    [JsonPropertyName("facebookUrl")]
    public string? FacebookUrl { get; set; }
    [JsonPropertyName("youtubeUrl")]
    public string? YoutubeUrl { get; set; }
    [JsonPropertyName("twitterUrl")]
    public string? TwitterUrl { get; set; }
    [JsonPropertyName("instagramUrl")]
    public string? InstagramUrl { get;}
}

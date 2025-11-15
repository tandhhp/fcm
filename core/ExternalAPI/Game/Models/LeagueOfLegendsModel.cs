using System.Text.Json.Serialization;

namespace Waffle.ExternalAPI.Game.Models
{
    public class LoL_Data<T> where T : class
    {
        [JsonPropertyName("data")]
        public IDictionary<string, T>? Data { get; set; }
    }

    public class LoL_Champion
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("blurb")]
        public string? Blurb { get; set; }
        [JsonPropertyName("version")]
        public string? Version { get; set; }
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new();
        [JsonPropertyName("image")]
        public Lol_Image Image { get; set; } = new();

    }

    public class Lol_Image
    {
        [JsonPropertyName("full")]
        public string? Full { get; set; }
    }
}

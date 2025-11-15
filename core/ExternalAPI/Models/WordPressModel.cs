using System.Text.Json.Serialization;

namespace Waffle.ExternalAPI.Models;

public class WordPressPost
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("slug")]
    public string? Slug { get; set; }
    [JsonPropertyName("link")]
    public string? Link { get; set; }
    [JsonPropertyName("title")]
    public WordPressTitle Title { get; set; } = new();
    [JsonPropertyName("content")]
    public WordPressContent Content { get; set; } = new();
    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }
    [JsonPropertyName("excerpt")]
    public WordPressExcerpt Excerpt { get; set; } = new();
    [JsonPropertyName("_embedded")]
    public Embedded Embedded { get; set; } = new();
}

public class Embedded
{
    [JsonPropertyName("wp:featuredmedia")]
    public List<FeaturedMedia> FeaturedMedia { get; set; } = new();
}

public class FeaturedMedia
{
    [JsonPropertyName("media_details")]
    public MediaDetails MediaDetails { get; set; } = new();
}

public class MediaDetails
{
    [JsonPropertyName("sizes")]
    public Sizes Sizes { get; set; } = new();
}

public class Sizes
{
    [JsonPropertyName("thumbnail")]
    public Thumbnail Thumbnail { get; set; } = new();
    [JsonPropertyName("medium_large")]
    public Thumbnail MediumLarge { get; set; } = new();
}

public class Thumbnail
{
    [JsonPropertyName("source_url")]
    public string? SourceUrl { get; set; }
}

public class WordPressTitle
{
    [JsonPropertyName("rendered")]
    public string? Rendered { get; set; }
}

public class WordPressContent
{
    [JsonPropertyName("rendered")]
    public string? Rendered { get; set; }
}

public class WordPressExcerpt
{
    [JsonPropertyName("rendered")]
    public string? Rendered { get; set; }
}

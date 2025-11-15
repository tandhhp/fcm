using System.Text.Json.Serialization;

namespace Waffle.ExternalAPI.Googles.Models;

public class BloggerListResult<T> where T : class
{
    [JsonPropertyName("nextPageToken")]
    public string? NextPageToken { get; set; }
    [JsonPropertyName("prevPageToken")]
    public string? PrevPageToken { get; set; }
    [JsonPropertyName("items")]
    public List<T>? Items { get; set; }
}
public class BloggerItem
{
    public BloggerItem()
    {
        Content = string.Empty;
        Url = string.Empty;
    }
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; }
    [JsonPropertyName("published")]
    public DateTime Published { get; set; }
    [JsonPropertyName("updated")]
    public DateTime Updated { get; set; }
    [JsonPropertyName("url")]
    public string Url { get; set; }
    [JsonPropertyName("images")]
    public List<BloggerImage> Images { get; set; } = new List<BloggerImage>();
    [JsonIgnore]
    public string Thumbnail => Images.Any() ? Images.First().Url : "https://placehold.jp/22254e/ffffff/350x240.png?text=IMAGE";
    public string Path
    {
        get
        {
            int indexOfSubstring = Url.LastIndexOf(".com") + 4;
            return Url[indexOfSubstring..];
        }
    }
    [JsonPropertyName("replies")]
    public BloggerReplies Replies { get; set; } = new BloggerReplies();
    [JsonPropertyName("labels")]
    public List<string> Labels { get; set; } = new List<string>();
}

public class BloggerImage
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}

public class BloggerReplies
{
    [JsonPropertyName("totalItems")]
    public string? TotalItems { get; set; }
}

public class BloggerComment
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }
    [JsonPropertyName("updated")]
    public DateTime Updated { get; set; }
    [JsonPropertyName("published")]
    public DateTime Published { get; set; }
    [JsonPropertyName("author")]
    public BloggerAuthor? Author { get; set; }
}

public class BloggerAuthor
{
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }
    [JsonPropertyName("image")]
    public BloggerImage? Image { get; set; }
}

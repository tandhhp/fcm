using System.Text.Json.Serialization;

namespace Waffle.ExternalAPI.Game.Models;

public class GIResult<T> where T : class
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}

public class GIContentList
{
    [JsonPropertyName("iTotal")]
    public int Total { get; set; }

    [JsonPropertyName("list")]
    public List<GIList> List { get; set; } = new();
}

public class GIList
{
    [JsonPropertyName("iInfoId")]
    public int InfoId { get; set; }
    [JsonPropertyName("sTitle")]
    public string? Title { get; set; }
}

public class GIContent
{
    [JsonPropertyName("sContent")]
    public string? Content { get; set; }
    [JsonPropertyName("sIntro")]
    public string? Intro { get; set; }
    [JsonPropertyName("sTitle")]
    public string? Title { get; set; }
}

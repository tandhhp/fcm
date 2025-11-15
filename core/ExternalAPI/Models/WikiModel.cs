using System.Text.Json.Serialization;
using Waffle.Core.Helpers;

namespace Waffle.ExternalAPI.Models
{
    public class Parse
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
    public class Action
    {
        [JsonPropertyName("parse")]
        public Parse? Parse { get; set; }
    }

    public class WikiQueryResult
    {
        [JsonPropertyName("query")]
        public WikiQuery Query { get; set; } = new();
    }

    public class WikiQuery
    {
        [JsonPropertyName("pages")]
        public List<WikiPage> Pages { get; set; } = new();
    }

    public class WikiPage
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("langlinks")]
        public List<WikiLangLink> LangLinks { get; set; } = new();
    }

    public class WikiLangLink
    {
        [JsonPropertyName("lang")]
        public string? Lang { get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonIgnore]
        public string Url => $"/wiki/{Lang}/{SeoHelper.ToWikiFriendly(Title)}";
    }
}

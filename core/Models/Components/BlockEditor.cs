using System.Text.Json.Serialization;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

public class BlockEditor : AbstractComponent
{
    [JsonPropertyName("blocks")]
    public List<BlockEditorBlock> Blocks { get; set; } = new();
    [JsonPropertyName("version")]
    public string? Version { get; set; }
}

public class BlockEditorBlock
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("data")]
    public BlockEditorItemData Data { get; set; } = new();
}

public class BlockEditorItemData 
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    [JsonPropertyName("level")]
    public int? Level { get; set; }
    [JsonPropertyName("html")]
    public string? Html { get; set; }
    [JsonPropertyName("link")]
    public string? Link { get; set; }
    [JsonPropertyName("code")]
    public string? Code { get; set; }
    [JsonPropertyName("meta")]
    public BlockEditorMeta Meta { get; set; } = new();
    [JsonPropertyName("items")]
    public List<string>? Items { get; set; }
    [JsonPropertyName("style")]
    public string Style { get; set; } = "ordered";
    [JsonPropertyName("url")]
    public string? Url { get; set; }
    [JsonPropertyName("caption")]
    public string? Caption { get; set; }
    [JsonPropertyName("withBackground")]
    public bool WithBackground { get; set; }
    [JsonPropertyName("blogger")]
    public Blogger Blogger { get; set; } = new();
    [JsonPropertyName("service")]
    public string? Service { get; set; }
    [JsonPropertyName("embed")]
    public string? Embed { get; set; }
}

public class BlockEditorMeta
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("image")]
    public BlockEditorImage Image { get; set; } = new();
}

public class BlockEditorImage
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

public class Blogger
{
    [JsonPropertyName("blogId")]
    public string? BlogId { get; set; }
    [JsonPropertyName("postId")]
    public string? PostId { get; set; }
}

public class BlockEditorType {
    public const string PARAGRAPH = "paragraph";
    public const string HEADER = "header";
    public const string DELIMITER = "delimiter";
    public const string RAW = "raw";
    public const string LINK_TOOL = "linkTool";
    public const string CODE = "code";
    public const string LIST = "list";
    public const string SIMPLE_IMAGE = "simpleImage";
    public const string QUOTE = "quote";
    public const string BLOGGER = "blogger";
    public const string YOUTUBE = "youtube";
    public const string EMBED = "embed";
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;
using Waffle.ExternalAPI.Models;

namespace Waffle.Models.Components;

[Display(Name = "Facebook album", Prompt = "facebook-album")]
public class FacebookAlbum : AbstractComponent
{
    [JsonPropertyName("albumId")]
    public string? AlbumId { get; set; }

    [JsonIgnore]
    public IEnumerable<FacebookPhoto>? Photos { get; set; }
    [JsonIgnore]
    public string? Before { get; set; }
    [JsonIgnore]
    public string? After { get; set; }
}

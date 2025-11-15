using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;
using Waffle.Entities;

namespace Waffle.Models.Components.Lister;

[Display(Name = "Album lister", Prompt = "album-lister")]
public class AlbumLister : AbstractComponent
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("albums")]
    public IEnumerable<Catalog> Albums { get; set; } = new List<Catalog>();

    [JsonPropertyName("source")]
    public string? Source { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

[Display(Name = "Tag", Prompt = "tag")]
public class Tag : AbstractComponent
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
}

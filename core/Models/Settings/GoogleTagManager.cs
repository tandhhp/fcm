using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;

namespace Waffle.Models.Settings;

[Display(Name = "Google Tag Manager", Prompt = "google-tag-manager")]
public class GoogleTagManager : BaseSetting
{
    [JsonPropertyName("tagId")]
    public string? TagId { get; set; }
}

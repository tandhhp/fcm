using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

[Display(Name = "Button", Prompt = "button", GroupName = GroupName.General)]
public class Button : AbstractComponent
{
    public Button()
    {
        Icon = string.Empty;
        Text = string.Empty;
        Type = "button";
    }

    [JsonPropertyName("icon")]
    public string Icon { get; set; }
    [JsonPropertyName("text")]
    public string Text { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonIgnore]
    public bool HasIcon => !string.IsNullOrEmpty(Icon);
}

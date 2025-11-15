using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;
using Waffle.Entities;
using Waffle.Models.Components;

namespace Waffle.Models.Settings;

[Display(Name = "Header", Prompt = "header", AutoGenerateFilter = true)]
public class Header : BaseSetting
{
    [JsonPropertyName("brand")]
    public string? Brand { get; set; }
    [JsonPropertyName("logo")]
    public string? Logo { get; set; }
    [JsonPropertyName("viewName")]
    public string ViewName { get; set; } = "Default";
    [JsonPropertyName("showInHome")]
    public bool ShowInHome { get; set; } = false;

    [JsonIgnore]
    public bool IsAuthenticated { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }

    [JsonIgnore]
    public Catalog Catalog { get; set; } = new();

    public IEnumerable<Option> Templates { get; set; }
     = new List<Option>
    {
        new Option { Label = "Blank", Value = "blank" },
        new Option { Label = "Default", Value = "default" },
        new Option { Label = "Dliti", Value = "dliti" }
    };
}

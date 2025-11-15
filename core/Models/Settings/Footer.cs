using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;
using Waffle.Models.Components;

namespace Waffle.Models.Settings;

[Display(Name = "Footer", Prompt = "footer", AutoGenerateFilter = true)]
public class Footer : BaseSetting
{
    [JsonPropertyName("companyName")]
    public string? CompanyName { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    [JsonPropertyName("social")]
    public Social Social { get; set; } = new();
    [JsonPropertyName("viewName")]
    public string ViewName { get; set; } = "Default";

    public IEnumerable<Option> Templates { get; set; } = new List<Option>
    {
        new Option { Label = "Blank", Value = "blank" },
        new Option { Label = "Default", Value = "default" }
    };
}

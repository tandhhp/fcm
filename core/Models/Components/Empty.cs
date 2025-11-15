using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

[Display(Name = "Empty", Prompt = "empty", AutoGenerateField = true, AutoGenerateFilter = true)]
public class Empty : AbstractComponent
{
    [JsonIgnore]
    public const string DefaultView = PartialViewName.Empty;
}

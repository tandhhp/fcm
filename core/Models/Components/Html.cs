using System.ComponentModel.DataAnnotations;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

[Display(Name = "Html", Prompt = "html")]
public class Html : AbstractComponent
{
    public string? Value { get; set; }
}

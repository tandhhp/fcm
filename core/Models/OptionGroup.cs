using Waffle.Models.Components;

namespace Waffle.Models;

public class OptionGroup
{
    public string? Label { get; set; }
    public object? Value { get; set; }
    public IEnumerable<Option>? Options { get; set; }
}

namespace Waffle.Core.Services.Settings.Models;

public class SaveSettingArgs
{
    public string Name { get; set; } = default!;
    public object? Value { get; set; }
}

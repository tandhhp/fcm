namespace Waffle.Core.Services.Sources.Args;

public class SourceCreateArgs
{
    public string Name { get; set; } = default!;
    public int TeamId { get; set; }
    public int TypeOfDataId { get; set; }
    public bool Protected { get; set; }
    public bool Overwrite { get; set; }
}

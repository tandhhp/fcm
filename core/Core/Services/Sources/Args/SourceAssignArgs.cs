namespace Waffle.Core.Services.Sources.Args;

public class SourceAssignArgs
{
    public List<int>? Sources { get; set; }
    public List<Assign> Assigns { get; set; } = [];
}

public class Assign
{
    public Guid TelesalesId { get; set; }
    public int NumberOfContact { get; set; }
}
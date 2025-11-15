namespace Waffle.Core.Services.Attendances.Args;

public class AttendanceCreateArgs
{
    public string Name { get; set; } = default!;
    public float SuRate { get; set; }
    public bool IsActive { get; set; }
}

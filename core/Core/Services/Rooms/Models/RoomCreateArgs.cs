namespace Waffle.Core.Services.Rooms.Models;

public class RoomCreateArgs
{
    public int BranchId { get; set; }
    public string Name { get; set; } = default!;
}

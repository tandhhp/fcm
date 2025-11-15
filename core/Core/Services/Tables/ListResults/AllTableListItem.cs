namespace Waffle.Core.Services.Tables.ListResults;

public class AllTableListItem
{
    public int TableId { get; set; }
    public string TableName { get; set; } = default!;
    public int SortOrder { get; set; }
    public bool Disabled { get; set; }
}

public class AllRoolListItem
{
    public int RoomId { get; set; }
    public string RoomName { get; set; } = default!;
    public List<AllTableListItem> Tables { get; set; } = [];
}
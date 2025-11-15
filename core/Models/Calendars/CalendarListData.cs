namespace Waffle.Models.Calendars;

public class CalendarListData
{
    public int Day { get; set; }
    public int EventCount { get; set; }
    public int PlasmaCount { get; set; }
    public List<CalendarListItem> Items { get; set; } = new List<CalendarListItem>();
}

public class CalendarListItem
{
    public string? Content { get; set; }
}

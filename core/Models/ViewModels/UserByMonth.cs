namespace Waffle.Models.ViewModels;

public class UserByMonth
{
    public Guid UserId { get; set; }
    public List<MonthAmount> Months { get; set; } = new();
    public string? Name { get; set; }
}

public class MonthAmount
{
    public int Month { get; set; }
    public decimal Amount { get; set; }
}

public class DayAmount
{
    public int Day { get; set; }
    public decimal Amount { get; set; }
}

public class UserByDay
{
    public Guid UserId { get; set; }
    public List<DayAmount> Days { get; set; } = new();
    public string? Name { get; set; }
}

namespace Waffle.Models.ViewModels
{
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
}

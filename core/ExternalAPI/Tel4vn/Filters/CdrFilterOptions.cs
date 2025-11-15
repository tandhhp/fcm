namespace Waffle.ExternalAPI.Tel4vn.Filters;

public class CdrFilterOptions
{
    public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-1);
    public DateTime EndDate { get; set; } = DateTime.Now;
    public int PageSize { get; set; } = 20;
    public int Current { get; set; } = 0;
    public string? Extension { get; set; }
    public string? Direction { get; set; }
    public string? Status { get; set; }
}

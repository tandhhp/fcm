namespace Waffle.Models.Filters;

public class ContactFilterOptions : FilterOptions
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? IsBooked { get; set; }
    public bool? Confirm1 { get; set; }
    public bool? Confirm2 { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

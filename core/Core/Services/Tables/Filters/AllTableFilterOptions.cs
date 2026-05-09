namespace Waffle.Core.Services.Tables.Filters;

public class AllTableFilterOptions
{
    public int? BranchId { get; set; }
    public Guid EventId { get; set; }
    public DateTime EventDate { get; set; }
}

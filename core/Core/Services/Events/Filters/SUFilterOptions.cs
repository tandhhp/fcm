using Waffle.Models;

namespace Waffle.Core.Services.Events.Filters;

public class SUFilterOptions : FilterOptions
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public Guid? SalesManagerId { get; set; }
    public Guid? DosId { get; set; }
    public Guid? DotId { get; set; }
}

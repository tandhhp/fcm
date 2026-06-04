using Waffle.Models;

namespace Waffle.Core.Services.Events.Filters;

public class SUFilterOptions : FilterOptions
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public Guid? ManagerId { get; set; }
    public Guid? DirectorId { get; set; }
}

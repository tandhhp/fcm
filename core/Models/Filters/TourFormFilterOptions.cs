using Waffle.Entities;
using Waffle.Entities.Tours;

namespace Waffle.Models.Filters;

public class TourFormFilterOptions : FilterOptions
{
    public CatalogType? Type { get; set; }
    public FormStatus? Status { get; set; }
    public string? FullName { get; set; }
    public string? ContractCode { get; set; }
}

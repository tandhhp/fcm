using Waffle.Models;

namespace Waffle.Core.Services.Contracts.Filters;

public class ContractInvoiceFilterOptions : FilterOptions
{
    public Guid ContractId { get; set; }
}

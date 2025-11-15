using Waffle.Models;

namespace Waffle.Core.Services.Contracts.Filters;

public class ContractGiftFilterOptions : FilterOptions
{
    public Guid ContractId { get; set; }
}

using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Branches;

namespace Waffle.Core.Interfaces.IRepository;

public interface IBranchRepository : IAsyncRepository<Branch>
{
    Task<ListResult<object>> ListAsync(BranchFilterOptions filterOptions);
    Task<object?> OptionsAsync(SelectOptions selectOptions);
}

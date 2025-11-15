using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Branches;

namespace Waffle.Core.Interfaces.IService;

public interface IBranchService
{
    Task<Branch?> FindAsync(int id);
    Task<ListResult<object>> ListAsync(BranchFilterOptions filterOptions);
    Task<object?> OptionsAsync(SelectOptions selectOptions);
}

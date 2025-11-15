using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Branches;

namespace Waffle.Core.Services.Branches;

public class BranchService(IBranchRepository _branchRepository) : IBranchService
{
    public Task<Branch?> FindAsync(int id) => _branchRepository.FindAsync(id);

    public Task<ListResult<object>> ListAsync(BranchFilterOptions filterOptions) => _branchRepository.ListAsync(filterOptions);

    public Task<object?> OptionsAsync(SelectOptions selectOptions) => _branchRepository.OptionsAsync(selectOptions);
}

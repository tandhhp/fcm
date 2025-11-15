using Waffle.Core.Services.Departments.Models;
using Waffle.Entities.Users;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService;

public interface IDepartmentService
{
    Task<TResult> CreateAsync(CreateDepartmentArgs args);
    Task<TResult> DeleteAsync(int id);
    Task<TResult<object>> DetailAsync(int id);
    Task<Department?> FindAsync(int id);
    Task<ListResult<object>> ListAsync(DepartmentFilterOptions filterOptions);
    Task<object?> OptionsAsync(SelectOptions selectOptions);
    Task<TResult> UpdateAsync(UpdateDepartmentArgs args);
}

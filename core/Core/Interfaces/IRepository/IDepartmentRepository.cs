using Waffle.Core.Services.Departments.Models;
using Waffle.Entities.Users;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository;

public interface IDepartmentRepository : IAsyncRepository<Department>
{
    Task<bool> HasTeamAsync(int id);
    Task<ListResult<object>> ListAsync(DepartmentFilterOptions filterOptions);
    Task<object?> OptionsAsync(SelectOptions selectOptions);
}

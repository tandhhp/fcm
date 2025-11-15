using Waffle.Core.Services.Attendances.Args;
using Waffle.Core.Services.Attendances.Filters;
using Waffle.Models;

namespace Waffle.Core.Services.Attendances;

public interface IAttendanceService
{
    Task<TResult> CreateAsync(AttendanceCreateArgs args);
    Task<TResult> DeleteAsync(int id);
    Task<object> GetOptionsAsync(SelectOptions selectOptions);
    Task<ListResult<object>> ListAsync(AttendanceFilterOptions filterOptions);
    Task<TResult> UpdateAsync(AttendanceUpdateArgs args);
}

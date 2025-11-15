using Waffle.Core.Services.Attendances.Filters;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Events;

public interface IAttendanceRepository : IAsyncRepository<Attendance>
{
    Task<object> GetOptionsAsync(SelectOptions selectOptions);
    Task<ListResult<object>> ListAsync(AttendanceFilterOptions filterOptions);
}

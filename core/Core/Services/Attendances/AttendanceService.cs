using Waffle.Core.Interfaces.IRepository.Events;
using Waffle.Core.Services.Attendances.Args;
using Waffle.Core.Services.Attendances.Filters;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Services.Attendances;

public class AttendanceService(IAttendanceRepository _attendanceRepository) : IAttendanceService
{
    public async Task<TResult> CreateAsync(AttendanceCreateArgs args)
    {
        await _attendanceRepository.AddAsync(new Attendance
        {
            Name = args.Name,
            SuRate = args.SuRate,
            IsActive = args.IsActive
        });
        return TResult.Success;
    }

    public async Task<TResult> DeleteAsync(int id)
    {
        var data = await _attendanceRepository.FindAsync(id);
        if (data is null) return TResult.Failed("Không tìm thấy dữ liệu!");
        await _attendanceRepository.DeleteAsync(data);
        return TResult.Success;
    }

    public Task<object> GetOptionsAsync(SelectOptions selectOptions) => _attendanceRepository.GetOptionsAsync(selectOptions);

    public Task<ListResult<object>> ListAsync(AttendanceFilterOptions filterOptions) => _attendanceRepository.ListAsync(filterOptions);

    public async Task<TResult> UpdateAsync(AttendanceUpdateArgs args)
    {
        var data = await _attendanceRepository.FindAsync(args.Id);
        if (data is null) return TResult.Failed("Không tìm thấy dữ liệu!");
        data.Name = args.Name;
        data.SuRate = args.SuRate;
        data.IsActive = args.IsActive;
        await _attendanceRepository.UpdateAsync(data);
        return TResult.Success;
    }
}

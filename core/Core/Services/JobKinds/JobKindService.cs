using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Services.JobKinds.Models;
using Waffle.Entities.Users;
using Waffle.Models;

namespace Waffle.Core.Services.JobKinds;

public class JobKindService(IJobKindRepository _jobKindRepository) : IJobKindService
{
    public async Task<TResult> CreateAsync(CreateJobKindArgs args)
    {
        await _jobKindRepository.AddAsync(new JobKind
        {
            IsActive = args.IsActive,
            Name = args.Name,
        });
        return TResult.Success;
    }

    public async Task<TResult> DeleteAsync(int id)
    {
        var jobKind = await _jobKindRepository.FindAsync(id);
        if (jobKind is null) return TResult.Failed("Không tìm thấy loại công việc!");
        if (await _jobKindRepository.IsUsedAsync(id)) return TResult.Failed("Không thể xóa loại công việc vì đã được sử dụng!");
        await _jobKindRepository.DeleteAsync(jobKind);
        return TResult.Success;
    }

    public async Task<TResult<object?>> GetAsync(int id)
    {
        var jobKind = await _jobKindRepository.FindAsync(id);
        if (jobKind is null) return TResult<object?>.Failed("Không tìm thấy loại công việc!");
        return TResult<object?>.Ok(new
        {
            jobKind.Id,
            jobKind.Name,
            jobKind.IsActive,
        });
    }

    public Task<ListResult<object>> ListAsync(FilterOptions filterOptions) => _jobKindRepository.ListAsync(filterOptions);

    public Task<object?> OptionsAsync() => _jobKindRepository.OptionsAsync();

    public async Task<TResult> UpdateAsync(UpdateJobKindArgs args)
    {
        var jobKind = await _jobKindRepository.FindAsync(args.Id);
        if (jobKind is null) return TResult.Failed("Không tìm thấy loại công việc!");
        jobKind.Name = args.Name;
        jobKind.IsActive = args.IsActive;
        await _jobKindRepository.UpdateAsync(jobKind);
        return TResult.Success;
    }
}

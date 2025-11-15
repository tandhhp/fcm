using Waffle.Core.Services.Events.Models;
using Waffle.Core.Services.Vouchers.Args;
using Waffle.Entities.Contracts;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService.Events;

public interface IVoucherService
{
    Task<TResult> CreateAsync(VoucherCreateArgs args);
    Task<TResult> DeleteAsync(Guid id);
    Task<TResult<object>> DetailAsync(Guid id);
    Task<TResult<byte[]?>> ExportAsync(VoucherFilterOptions filterOptions);
    Task<Voucher?> FindAsync(Guid id);
    Task<object?> GetOptionsAsync(VoucherSelectOptions selectOptions);
    Task<TResult> ImportAsync(VoucherImportArgs args);
    Task<bool> IsUsedAsync(Guid id);
    Task<ListResult<object>> ListAsync(VoucherFilterOptions filterOptions);
    Task<TResult> UpdateAsync(VoucherUpdateArgs args);
}

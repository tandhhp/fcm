using Waffle.Core.Services.Events.Models;
using Waffle.Core.Services.Vouchers.Results;
using Waffle.Entities.Contracts;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Events;

public interface IVoucherRepository : IAsyncRepository<Voucher>
{
    Task<List<VoucherExportResult>> GetExportDataAsync(VoucherFilterOptions filterOptions);
    Task<object?> GetOptionsAsync(VoucherSelectOptions selectOptions);
    Task<bool> IsUsedAsync(Guid id);
    Task<ListResult<object>> ListAsync(VoucherFilterOptions filterOptions);
}

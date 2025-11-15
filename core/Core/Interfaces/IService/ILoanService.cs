using Waffle.Models;
using Waffle.Models.Args.Users;
using Waffle.Models.Filters.Users;

namespace Waffle.Core.Interfaces.IService;

public interface ILoanService
{
    Task<TResult> LoanPointAsync(LoanPointArgs args);
    Task<ListResult<object>> LoanListAsync(LoanFilterOptions filterOptions);
    Task<TResult> ApproveLoanAsync(ApproveLoanArgs args);
}

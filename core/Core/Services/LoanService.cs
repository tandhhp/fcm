using Microsoft.AspNetCore.Identity;
using Waffle.Core.Interfaces;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Payments;
using Waffle.Models;
using Waffle.Models.Args.Users;
using Waffle.Models.Filters.Users;

namespace Waffle.Core.Services;

public class LoanService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IHCAService _hcaService, ILogService logService) : ILoanService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;
    private readonly ILogService _logService = logService;

    public async Task<TResult> LoanPointAsync(LoanPointArgs args)
    {
        var user = await _userManager.FindByIdAsync(args.UserId);
        if (user is null) return TResult.Failed("Người dùng không tồn tại");
        if (args.Point <= 0) return TResult.Failed("Số điểm vay không hợp lệ");
        await _context.Transactions.AddAsync(new Transaction
        {
            UserId = user.Id,
            Type = TransactionType.Loan,
            CreatedDate = DateTime.Now,
            Point = args.Point,
            Status = TransactionStatus.Pending,
            CreatedBy = _hcaService.GetUserId()
        });
        await _logService.AddAsync($"Người dùng {user.Name} ({user.ContractCode}) đã vay {args.Point} điểm");
        await _context.SaveChangesAsync();
        return TResult.Success;
    }

    public async Task<ListResult<object>> LoanListAsync(LoanFilterOptions filterOptions)
    {
        var query = from a in _context.Transactions
                    where a.Type == TransactionType.Loan
                    select new
                    {
                        a.Id,
                        a.Point,
                        a.CreatedDate,
                        a.Status,
                        a.Reason,
                        a.Feedback,
                        a.ApprovedDate,
                        a.UserId
                    };
        if (filterOptions.UserId != null)
        {
            query = query.Where(x => x.UserId == filterOptions.UserId);
        }
        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<TResult> ApproveLoanAsync(ApproveLoanArgs args)
    {
        var transaction = await _context.Transactions.FindAsync(args.TransactionId);
        if (transaction is null) return TResult.Failed("Không tìm thấy giao dịch!");
        transaction.Status = args.Status;
        transaction.Reason = args.Reason;
        transaction.ApprovedDate = DateTime.Now;
        transaction.ApprovedBy = _hcaService.GetUserId();
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
        return TResult.Success;
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waffle.Core.Constants;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Payments;
using Waffle.Extensions;
using Waffle.Foundations;
using Waffle.Models;
using Waffle.Models.Filters;
using Waffle.Models.ViewModels;

namespace Waffle.Controllers;

public class ReportController(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager, IHCAService _hcaService) : BaseController
{
    [HttpGet("amounts")]
    public async Task<IActionResult> AmountsAsync()
    {
        var userId = _hcaService.GetUserId();
        var query = _context.Invoices.AsNoTracking().Where(x => x.Amount > 0).Select(x => new
        {
            x.Amount,
            x.Status,
            x.CreatedAt,
            x.SalesId
        });
        if (_hcaService.IsUserInRole(RoleName.Sales))
        {
            query = query.Where(x => x.SalesId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.SalesManager))
        {
            var salesIds = await _context.Users.Where(x => x.SmId == userId).Select(x => x.Id).ToListAsync();
            query = query.Where(x => salesIds.Contains(x.SalesId));
        }
        var total = await query.Where(x => x.Status == InvoiceStatus.Approved).SumAsync(x => x.Amount);
        var year = await query.Where(x => x.Status == InvoiceStatus.Approved).Where(x => x.CreatedAt.Year == DateTime.Now.Year).SumAsync(x => x.Amount);
        var current = await query.Where(x => x.Status == InvoiceStatus.Approved).Where(x => x.CreatedAt.Year == DateTime.Now.Year && x.CreatedAt.Month == DateTime.Now.Month).SumAsync(x => x.Amount);
        var prev = await query.Where(x => x.Status == InvoiceStatus.Approved).Where(x => x.CreatedAt.Year == DateTime.Now.Year && x.CreatedAt.Month == DateTime.Now.AddMonths(-1).Month).SumAsync(x => x.Amount);
        var totalPending = await query.Where(x => x.Status == InvoiceStatus.Pending).SumAsync(x => x.Amount);
        var currentPending = await query.Where(x => x.Status == InvoiceStatus.Pending).Where(x => x.CreatedAt.Year == DateTime.Now.Year && x.CreatedAt.Month == DateTime.Now.Month).SumAsync(x => x.Amount);
        var prevPending = await query.Where(x => x.Status == InvoiceStatus.Pending).Where(x => x.CreatedAt.Year == DateTime.Now.Year && x.CreatedAt.Month == DateTime.Now.AddMonths(-1).Month).SumAsync(x => x.Amount);
        var yearPending = await query.Where(x => x.Status == InvoiceStatus.Pending).Where(x => x.CreatedAt.Year == DateTime.Now.Year).SumAsync(x => x.Amount);

        return Ok(new
        {
            data = new
            {
                total,
                year,
                current,
                prev,
                totalPending,
                currentPending,
                prevPending,
                yearPending
            }
        });
    }

    [HttpGet("year-chart-line")]
    public async Task<IActionResult> YearLineChartAsync([FromQuery] int year, [FromQuery] int? branch)
    {
        var userId = _hcaService.GetUserId();
        var query = from a in _context.Users
                    join b in _context.Invoices on a.Id equals b.SalesId
                    where b.Amount > 0 && b.Status == InvoiceStatus.Approved && b.CreatedAt.Year == year
                    select new
                    {
                        b.Id,
                        b.SalesId,
                        b.Amount,
                        a.Name,
                        a.UserName,
                        a.Email,
                        b.CreatedAt,
                        a.SmId,
                        a.DosId,
                        a.BranchId
                    };
        if (User.IsInRole(RoleName.Sales))
        {
            query = query.Where(x => x.SalesId == userId);
        }
        if (User.IsInRole(RoleName.SalesManager))
        {
            query = query.Where(x => x.SmId == userId);
        }
        if (User.IsInRole(RoleName.Dos))
        {
            query = query.Where(x => x.DosId == userId);
        }
        if (branch != null)
        {
            query = query.Where(x => x.BranchId == branch);
        }
        var data = await query.ToListAsync();
        var result = new List<MonthAmount>();
        for (int i = 1; i <= 12; i++)
        {
            result.Add(new MonthAmount
            {
                Month = i,
                Amount = data.Where(x => x.CreatedAt.Month == i).Sum(x => x.Amount)
            });
        }
        return Ok(result);
    }

    [HttpGet("sales")]
    public async Task<IActionResult> SalesAsync([FromQuery] int year)
    {
        var userId = _hcaService.GetUserId();
        var query = from a in _context.Users.Where(x => x.Status == UserStatus.Working)
                    join b in _context.Invoices on a.Id equals b.SalesId
                    where b.Amount > 0 && b.Status == InvoiceStatus.Approved && b.CreatedAt.Year == year
                    select new
                    {
                        b.Id,
                        b.SalesId,
                        b.Amount,
                        a.Name,
                        a.UserName,
                        a.Email,
                        b.CreatedAt,
                        a.SmId,
                        a.DosId
                    };
        if (User.IsInRole(RoleName.Sales))
        {
            query = query.Where(x => x.SalesId == userId);
        }
        if (User.IsInRole(RoleName.SalesManager))
        {
            query = query.Where(x => x.SmId == userId);
        }
        if (User.IsInRole(RoleName.Dos))
        {
            query = query.Where(x => x.DosId == userId);
        }
        var data = await query.ToListAsync();
        var sales = await _userManager.GetUsersInRoleAsync(RoleName.Sales);
        var result = new List<UserByMonth>();
        foreach (var sale in sales)
        {
            var months = new List<MonthAmount>();
            for (int i = 1; i <= 12; i++)
            {
                months.Add(new MonthAmount
                {
                    Month = i,
                    Amount = data.Where(x => x.CreatedAt.Month == i && x.SalesId == sale.Id).Sum(x => x.Amount)
                });
            }
            result.Add(new UserByMonth
            {
                UserId = sale.Id,
                Name = sale.Name,
                Months = months
            });
        }
        return Ok(result);
    }

    [HttpGet("times")]
    public async Task<IActionResult> TimesAsync([FromQuery] int year)
    {
        var query = _context.UserTopups.Where(x => x.Amount > 0)
            .Where(x => x.CreatedDate.Year == year);
        if (User.IsInRole(RoleName.SalesManager))
        {
            query = query.Where(x => x.SmId == User.GetId());
        }
        if (User.IsInRole(RoleName.Dos))
        {
            query = query.Where(x => x.DosId == User.GetId());
        }
        var data = await query.ToListAsync();
        return Ok(data.GroupBy(x => x.CreatedDate.Month).Select(x => new
        {
            x.Key,
            Amount = x.Sum(s => s.Amount)
        }));
    }

    [HttpGet("keyin-by-telesale")]
    public async Task<IActionResult> ListKeyInByTeleSales([FromQuery] BasicFilterOptions filterOptions)
    {
        try
        {
            var userId = User.GetId();
            var query = from a in _context.Users
                        where a.TmId == userId
                        select new
                        {
                            a.Id,
                            a.UserName,
                            a.Name,
                            a.Gender,
                            a.PhoneNumber,
                            leadCount = _context.Leads.Count(x => x.TelesaleId == a.Id),
                            a.TmId,
                            a.DotId
                        };
            if (User.IsInRole(RoleName.SalesManager))
            {
                query = query.Where(x => x.TmId == userId);
            }
            if (User.IsInRole(RoleName.Dot))
            {
                query = query.Where(x => x.DotId == userId);
            }
            return Ok(new
            {
                data = await query.Skip((filterOptions.Current - 1) * filterOptions.PageSize).Take(filterOptions.PageSize).ToListAsync(),
                total = await query.CountAsync()
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("loyalty")]
    public async Task<IActionResult> ExportLoyaltyAsync([FromQuery] LoyaltyFilterOptions filterOptions)
    {
        try
        {
            var query = from a in _context.Users
                        select new
                        {
                            a.Id,
                            a.UserName,
                            a.Name,
                            a.ContractCode,
                            a.Loyalty,
                            a.Token,
                            a.LoanPoint,
                            Deposit = _context.Transactions.Where(x => x.UserId == a.Id && x.Type == TransactionType.Default && x.CreatedDate >= filterOptions.FromDate && x.CreatedDate <= filterOptions.FromDate && x.Status == TransactionStatus.Approved && x.Point > 0).Sum(x => x.Point),
                            Withdraw = _context.Transactions.Where(x => x.UserId == a.Id && x.Type == TransactionType.Default && x.CreatedDate >= filterOptions.FromDate && x.CreatedDate <= filterOptions.FromDate && x.Status == TransactionStatus.Approved && x.Point < 0).Sum(x => x.Point)
                        };
            if (!string.IsNullOrWhiteSpace(filterOptions.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(filterOptions.ContractCode))
            {
                query = query.Where(x => x.UserName.ToLower().Contains(filterOptions.ContractCode.ToLower()));
            }
            query = query.OrderByDescending(x => x.UserName);
            return Ok(await ListResult<object>.Success(query, filterOptions));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}

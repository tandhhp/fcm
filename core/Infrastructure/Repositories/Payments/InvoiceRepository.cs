using Microsoft.EntityFrameworkCore;
using System.IO;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository.Finances;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Finances.Invoices.Filters;
using Waffle.Core.Services.Finances.Invoices.Results;
using Waffle.Data;
using Waffle.Entities.Payments;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Payments;

public class InvoiceRepository(ApplicationDbContext context, IHCAService _hcaService) : EfRepository<Invoice>(context), IInvoiceRepository
{
    public async Task<List<InvoiceExportListItem>> GetExportListAsync(InvoiceExportFilterOptions filterOptions)
    {
        var query = from i in _context.Invoices
                    join c in _context.Contracts on i.ContractId equals c.Id
                    join l in _context.Leads on c.LeadId equals l.Id
                    join s in _context.Users on i.SalesId equals s.Id
                    join sm in _context.Users on c.SalesId equals sm.Id into sc
                    from ssm in sc.DefaultIfEmpty()
                    join to in _context.Users on c.ToById equals to.Id into tc
                    from sto in tc.DefaultIfEmpty()
                    select new InvoiceExportListItem
                    {
                        Id = i.Id,
                        InvoiceNumber = i.InvoiceNumber,
                        Amount = i.Amount,
                        Status = i.Status,
                        CreatedAt = i.CreatedAt,
                        Note = i.Note,
                        EvidenceUrl = i.EvidenceUrl,
                        SalesId = i.SalesId,
                        ContractId = i.ContractId,
                        ContractCode = c.Code,
                        PaymentMethod = i.PaymentMethod,
                        SalesName = s.Name,
                        CustomerName = l.Name,
                        SalesManagerName = ssm.Name,
                        TO = sto.Name
                    };
        if (filterOptions.Status.HasValue)
        {
            query = query.Where(x => x.Status == filterOptions.Status);
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.InvoiceNumber))
        {
            query = query.Where(i => i.InvoiceNumber != null && i.InvoiceNumber.Contains(filterOptions.InvoiceNumber));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.ContractCode))
        {
            query = query.Where(i => i.ContractCode != null && i.ContractCode.ToLower().Contains(filterOptions.ContractCode.ToLower()));
        }
        if (filterOptions.FromDate != null && filterOptions.ToDate != null)
        {
            query = query.Where(i => i.CreatedAt.Date >= filterOptions.FromDate.Value.Date && i.CreatedAt.Date <= filterOptions.ToDate.Value.Date);
        }
        query = query.OrderByDescending(i => i.CreatedAt);
        return await query.ToListAsync();
    }

    public async Task<string?> GetLastInvoiceNumberAsync() => await _context.Invoices
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => i.InvoiceNumber)
            .FirstOrDefaultAsync();

    public async Task<ListResult<InvoiceListItem>> ListAsync(InvoiceFilterOptions filterOptions)
    {
        var userId = _hcaService.GetUserId();
        var query = from i in _context.Invoices
                    join c in _context.Contracts on i.ContractId equals c.Id
                    join s in _context.Users on i.SalesId equals s.Id
                    select new InvoiceListItem
                    {
                        Id = i.Id,
                        InvoiceNumber = i.InvoiceNumber,
                        Amount = i.Amount,
                        Status = i.Status,
                        CreatedAt = i.CreatedAt,
                        Note = i.Note,
                        EvidenceUrl = i.EvidenceUrl,
                        SalesId = i.SalesId,
                        ContractId = i.ContractId,
                        ContractCode = c.Code,
                        PaymentMethod = i.PaymentMethod,
                        SalesName = s.Name,
                        SaleManagerId = s.SmId,
                        DosId = s.DosId
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.InvoiceNumber))
        {
            query = query.Where(i => i.InvoiceNumber != null && i.InvoiceNumber.Contains(filterOptions.InvoiceNumber));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.ContractCode))
        {
            query = query.Where(i => i.ContractCode != null && i.ContractCode.ToLower().Contains(filterOptions.ContractCode.ToLower()));
        }
        if (_hcaService.IsUserInRole(RoleName.Sales))
        {
            query = query.Where(i => i.SalesId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.SalesManager))
        {
            query = query.Where(i => i.SaleManagerId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.Dos))
        {
            query = query.Where(i => i.DosId == userId);
        }
        if (filterOptions.Status.HasValue)
        {
            query = query.Where(x => x.Status == filterOptions.Status);
        }
        if (filterOptions.FromDate != null && filterOptions.ToDate != null)
        {
            query = query.Where(i => i.CreatedAt.Date >= filterOptions.FromDate.Value.Date && i.CreatedAt.Date <= filterOptions.ToDate.Value.Date);
        }
        query = query.OrderByDescending(i => i.CreatedAt);
        return await ListResult<InvoiceListItem>.Success(query, filterOptions);
    }

    public async Task<TResult<object>> StatisticsAsync()
    {
        var now = DateTime.Today;
        var dailyCount = await _context.Invoices.CountAsync(x => x.CreatedAt.Date == now);
        var weekStart = now.AddDays(-(int)now.DayOfWeek);
        var weeklyCount = await _context.Invoices.CountAsync(x => x.CreatedAt.Date >= weekStart);
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var monthlyCount = await _context.Invoices.CountAsync(x => x.CreatedAt.Date >= monthStart);
        var dailyAmount = await _context.Invoices.Where(x => x.CreatedAt.Date == now && x.Status == InvoiceStatus.Approved).SumAsync(x => x.Amount);
        var weeklyAmount = await _context.Invoices.Where(x => x.CreatedAt.Date >= weekStart && x.Status == InvoiceStatus.Approved).SumAsync(x => x.Amount);
        var monthlyAmount = await _context.Invoices.Where(x => x.CreatedAt.Date >= monthStart && x.Status == InvoiceStatus.Approved).SumAsync(x => x.Amount);
        return TResult<object>.Ok(new
        {
            weeklyAmount,
            monthlyAmount,
            dailyAmount,
            dailyCount,
            weeklyCount,
            monthlyCount
        });
    }
}

using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository.Events;
using Waffle.Core.Services.Events.Models;
using Waffle.Core.Services.Vouchers.Results;
using Waffle.Data;
using Waffle.Entities.Contracts;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Events;

public class VoucherRepository(ApplicationDbContext context) : EfRepository<Voucher>(context), IVoucherRepository
{
    public async Task<List<VoucherExportResult>> GetExportDataAsync(VoucherFilterOptions filterOptions)
    {
        var query1 = from l in _context.Leads
                    join v in _context.Vouchers on l.Voucher1Id equals v.Id
                    into vl from v in vl.DefaultIfEmpty()
                    join c in _context.Campaigns on v.CampaignId equals c.Id
                    select new VoucherExportResult
                    {
                        ActivedAt = v.ActiveAt,
                        CustomerIdNumber = l.IdentityNumber,
                        CustomerName = l.Name,
                        CustomerPhoneNumber = l.PhoneNumber,
                        Code = v.Name,
                        ExpiredAt = v.ExpiredDate,
                        ExpiredDays = v.ExpiredDays,
                        Status = v.Status,
                        Note = v.Note,
                        CampaignName = c.Name
                    };
        var voucher1 = await query1.ToListAsync();

        var query2 = from l in _context.Leads
                    join v in _context.Vouchers on l.Voucher2Id equals v.Id into vl
                    from v in vl.DefaultIfEmpty()
                    join c in _context.Campaigns on v.CampaignId equals c.Id
                    select new VoucherExportResult
                    {
                        ActivedAt = v.ActiveAt,
                        CustomerIdNumber = l.IdentityNumber,
                        CustomerName = l.Name,
                        CustomerPhoneNumber = l.PhoneNumber,
                        Code = v.Name,
                        ExpiredAt = v.ExpiredDate,
                        ExpiredDays = v.ExpiredDays,
                        Status = v.Status,
                        Note = v.Note,
                        CampaignName = c.Name
                    };
        var voucher2 = await query2.ToListAsync();

        return [.. voucher1, .. voucher2];
    }

    public async Task<object?> GetOptionsAsync(VoucherSelectOptions selectOptions)
    {
        var query = from a in _context.Vouchers
                    select new
                    {
                        a.Id,
                        a.Name,
                        a.CampaignId,
                        IsUsed = a.Status != VoucherStatus.Unused
                    };
        if (selectOptions.CampaignId.HasValue)
        {
            query = query.Where(x => x.CampaignId == selectOptions.CampaignId);
        }
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        return await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id,
            Disabled = x.IsUsed
        }).ToListAsync();
    }

    public async Task<bool> IsUsedAsync(Guid id)
    {
        return await _context.Leads.AnyAsync(x => x.Voucher1Id == id) || await _context.Leads.AnyAsync(x => x.Voucher2Id == id);
    }

    public async Task<ListResult<object>> ListAsync(VoucherFilterOptions filterOptions)
    {
        var query = from a in _context.Vouchers
                    select new
                    {
                        a.Id,
                        a.CampaignId,
                        a.ActiveAt,
                        a.Name,
                        IsUsed = a.ActiveAt != null,
                        IsExpired = a.ExpiredDate < DateTime.Now,
                        a.ExpiredDays,
                        a.ExpiredDate,
                        a.Status,
                        a.Note,
                        CustomerName = _context.Leads.First(x => x.Voucher1Id == a.Id || x.Voucher2Id == a.Id).Name,
                        CustomerPhone = _context.Leads.First(x => x.Voucher1Id == a.Id || x.Voucher2Id == a.Id).PhoneNumber,
                        CustomerIdNumber = _context.Leads.First(x => x.Voucher1Id == a.Id || x.Voucher2Id == a.Id).IdentityNumber
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        if (filterOptions.IsUsed.HasValue)
        {
            query = query.Where(x => x.IsUsed == filterOptions.IsUsed);
        }
        if (filterOptions.IsExpired.HasValue)
        {
            query = query.Where(x => x.IsExpired == filterOptions.IsExpired);
        }
        if (filterOptions.Status.HasValue)
        {
            query = query.Where(x => x.Status == filterOptions.Status);
        }
        query = query.OrderByDescending(x => x.ActiveAt);
        return await ListResult<object>.Success(query, filterOptions);
    }
}

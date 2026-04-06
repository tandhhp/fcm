using Microsoft.EntityFrameworkCore;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Contracts.Args;
using Waffle.Core.Services.Contracts.Filters;
using Waffle.Core.Services.Contracts.Results;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Contracts;
using Waffle.Entities.Payments;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories;

public class ContractRepository(ApplicationDbContext context, IHCAService _hcaService) : EfRepository<Contract>(context), IContractRepository
{
    public async Task<TResult> AddEvidencesAsync(List<Evidence> evidences)
    {
        await _context.Evidences.AddRangeAsync(evidences);
        await _context.SaveChangesAsync();
        return TResult.Success;
    }

    public async Task<bool> AnyAsync(string contractCode) => await _context.Contracts.AnyAsync(c => c.Code.ToLower() == contractCode.ToLower());

    public async Task<TResult> CreatePaymentAsync(ContractCreatePayment args, Guid salesId)
    {
        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            ContractId = args.ContractId,
            Amount = args.Amount,
            Note = args.Note,
            CreatedBy = _hcaService.GetUserId(),
            CreatedAt = args.CreatedDate,
            Status = InvoiceStatus.Pending,
            InvoiceNumber = args.InvoiceNumber,
            SalesId = salesId,
            PaymentMethod = args.PaymentMethod
        };
        if (args.EvidenceUrls != null && args.EvidenceUrls.Count > 0)
        {
            await _context.Evidences.AddRangeAsync(args.EvidenceUrls.Select(url => new Evidence
            {
                Id = Guid.NewGuid(),
                ContractId = args.ContractId,
                Url = url,
                FileName = System.IO.Path.GetFileName(url),
                UploadAt = DateTime.UtcNow,
                UploaderId = _hcaService.GetUserId(),
                EvidenceTypeId = 3,
                InvoiceId = invoice.Id
            }));
        }
        await _context.Invoices.AddAsync(invoice);
        await _context.SaveChangesAsync();
        return TResult.Success;
    }

    public async Task<TResult> DeleteEvidenceAsync(Guid id)
    {
        var evidence = await _context.Evidences.FindAsync(id);
        if (evidence is null)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice is null) return TResult.Failed("Không tìm thấy chứng từ!");
            invoice.EvidenceUrl = null;
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
            return TResult.Success;
        }
        _context.Evidences.Remove(evidence);
        await _context.SaveChangesAsync();
        return TResult.Success;
    }

    public async Task DeleteInvoicesAsync(Guid id)
    {
        var invoices = await _context.Invoices.Where(i => i.ContractId == id).ToListAsync();
        if (invoices.Count == 0) return;
        _context.Invoices.RemoveRange(invoices);
    }

    public async Task<TResult<object>> GetEvidencesAsync(Guid contractId)
    {
        try
        {
            var contract = await _context.Contracts.FindAsync(contractId);
            if (contract is null) return TResult<object>.Failed("Không tìm thấy hợp đồng!");
            var query = from e in _context.Evidences
                        where e.ContractId == contractId
                        select new
                        {
                            e.Id,
                            e.Url,
                            e.FileName,
                            e.UploadAt,
                            e.UploaderId,
                            e.EvidenceTypeId
                        };
            var evidenceTypes = await _context.EvidenceTypes.AsNoTracking().ToListAsync();
            var data = await query.OrderByDescending(e => e.UploadAt).ToListAsync();
            var invoices = await _context.Invoices.Where(i => i.ContractId == contractId && !string.IsNullOrEmpty(i.EvidenceUrl)).AsNoTracking().ToListAsync();
            var result = evidenceTypes.Select(x =>
            {
                var evidences = data.Where(d => d.EvidenceTypeId == x.Id).Select(e => new
                {
                    e.Id,
                    e.Url,
                    e.FileName,
                    e.UploadAt,
                    e.UploaderId
                }).ToList();
                if (x.Name == "Chứng từ thu tiền")
                {
                    if (invoices.Count > 0)
                    {
                        evidences.AddRange(invoices.Select(i => new
                        {
                            i.Id,
                            Url = i.EvidenceUrl!,
                            FileName = $"INV",
                            UploadAt = i.CreatedAt,
                            UploaderId = i.CreatedBy
                        }));
                    }
                }
                return new
                {
                    Key = x.Name,
                    Evidences = evidences
                };
            }).ToList();
            return TResult<object>.Ok(result);
        }
        catch (Exception ex)
        {
            return TResult<object>.Failed(ex.ToString());
        }
    }

    public async Task<object> GetEvidenceTypeOptionsAsync() => await _context.EvidenceTypes.Select(x => new
    {
        Label = x.Name,
        Value = x.Id
    }).ToListAsync();

    public async Task<List<ContractExportResult>> GetExportDataAsync(ContractFilterOptions filterOptions)
    {
        var query = from c in _context.Contracts
                    join l in _context.Leads on c.LeadId equals l.Id
                    join creator in _context.Users on c.KeyInId equals creator.Id
                    join sales in _context.Users on c.SalesId equals sales.Id into salesJoin
                    from sales in salesJoin.DefaultIfEmpty()
                    join card in _context.Cards on c.CardId equals card.Id into cardJoin
                    from card in cardJoin.DefaultIfEmpty()
                    join to in _context.Users on c.ToById equals to.Id into toJoin
                    from to in toJoin.DefaultIfEmpty()
                    select new ContractExportResult
                    {
                        ContractCode = c.Code,
                        CreatedAt = c.CreatedDate,
                        TotalAmount = c.Amount,
                        CardName = card.Name,
                        AmountPaid = _context.Invoices.Where(i => i.ContractId == c.Id && i.Status == InvoiceStatus.Approved).Sum(i => i.Amount),
                        CustomerName = l.Name,
                        CustomerIdNumber = l.IdentityNumber,
                        CustomerPhone = l.PhoneNumber,
                        SalesName = sales.Name,
                        TOName = to.Name,
                        AmountPending = _context.Invoices.Where(i => i.ContractId == c.Id && (i.Status == InvoiceStatus.Pending || i.Status == InvoiceStatus.SAConfirmed)).Sum(i => i.Amount),
                        DOS = sales != null && sales.DosId != null ? _context.Users.First(x => x.Id == sales.Id).Name : string.Empty,
                        SM = sales != null && sales.SmId != null ? _context.Users.First(x => x.Id == sales.SmId).Name : string.Empty,
                        KeyIn = creator.Name,
                        TeamKeyIn = creator.ManagerId != null ? _context.Users.First(x => x.Id == creator.ManagerId).Name : string.Empty,
                        Discount = _context.Coupons.Where(cp => cp.ContractId == c.Id).Sum(cp => cp.Discount),
                    };
        if (filterOptions.FromDate.HasValue && filterOptions.ToDate.HasValue)
        {
            query = query.Where(c => c.CreatedAt.Date >= filterOptions.FromDate.Value.Date && c.CreatedAt.Date <= filterOptions.ToDate.Value.Date);
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.ContractCode))
        {
            query = query.Where(c => c.ContractCode != null && c.ContractCode.ToLower().Contains(filterOptions.ContractCode.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(c => c.CustomerPhone != null && c.CustomerPhone.ToLower().Contains(filterOptions.PhoneNumber.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.IdentityNumber))
        {
            query = query.Where(c => c.CustomerIdNumber != null && c.CustomerIdNumber.ToLower().Contains(filterOptions.IdentityNumber.ToLower()));
        }
        return await query.ToListAsync();
    }

    public async Task<object> GetInvoiceOptionsAsync(Guid contractId)
    {
        return await _context.Invoices.Where(i => i.ContractId == contractId).Select(i => new
        {
            Label = $"{i.InvoiceNumber} - {i.Amount:N0} VND",
            Value = i.Id
        }).ToListAsync();
    }

    public async Task<ListResult<object>> GetInvoicesAsync(ContractInvoiceFilterOptions filterOptions)
    {
        var query = from i in _context.Invoices
                    where i.ContractId == filterOptions.ContractId
                    select new
                    {
                        i.Id,
                        i.InvoiceNumber,
                        i.Amount,
                        i.Status,
                        i.Note,
                        i.CreatedAt,
                        i.EvidenceUrl,
                        i.PaymentMethod
                    };
        query = query.OrderByDescending(i => i.CreatedAt);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<object?> GetLeadOptionsAsync(ContactLeadSelectOptions selectOptions)
    {
        var query = from l in _context.Leads
                    where !string.IsNullOrEmpty(l.IdentityNumber) && !string.IsNullOrEmpty(l.PhoneNumber) && (l.Status == LeadStatus.Checkin || l.Status == LeadStatus.CloseDeal)
                    select new
                    {
                        l.Id,
                        l.Name,
                        l.PhoneNumber,
                        l.IdentityNumber
                    };
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(l => l.IdentityNumber.Contains(selectOptions.KeyWords) || l.PhoneNumber.Contains(selectOptions.KeyWords));
        }
        return await query.Select(x => new
        {
            Label = $"{x.IdentityNumber} - {x.PhoneNumber} - {x.Name}",
            Value = x.Id,
        }).ToListAsync();
    }

    public async Task<decimal> GetTotalAmountPaidAsync(Guid contractId) => await _context.Invoices.Where(x => x.ContractId == contractId && x.Status != InvoiceStatus.Rejected && x.Status != InvoiceStatus.Cancelled).SumAsync(x => x.Amount);

    public async Task<TResult> GiftContractAsync(ContractGiftArgs args)
    {
        await _context.Gifts.AddAsync(new Gift
        {
            Name = args.Name,
            ContractId = args.ContractId,
            Amount = args.Amount,
            ExpiredDate = args.ExpiredDate,
            CreatedAt = DateTime.UtcNow,
            UserId = _hcaService.GetUserId()
        });
        await _context.SaveChangesAsync();
        return TResult.Success;
    }

    public async Task<ListResult<object>> ListAsync(ContractFilterOptions filterOptions)
    {
        var userId = _hcaService.GetUserId();
        var query = from c in _context.Contracts
                    join l in _context.Leads on c.LeadId equals l.Id
                    join sales in _context.Users on c.SalesId equals sales.Id into salesJoin
                    from sales in salesJoin.DefaultIfEmpty()
                    join s in _context.Sources on c.SourceId equals s.Id into sourceJoin
                    from s in sourceJoin.DefaultIfEmpty()
                    select new
                    {
                        c.Id,
                        c.CreatedDate,
                        c.CreatedBy,
                        ContractCode = c.Code,
                        InvoiceCount = _context.Invoices.Count(i => i.ContractId == c.Id),
                        c.Amount,
                        PaidAmount = _context.Invoices.Where(i => i.ContractId == c.Id && i.Status == InvoiceStatus.Approved).Sum(i => i.Amount),
                        CustomerName = l.Name,
                        l.Gender,
                        l.PhoneNumber,
                        l.IdentityNumber,
                        l.DateOfBirth,
                        SalesName = sales.Name,
                        PendingAmount = _context.Invoices.Where(i => i.ContractId == c.Id && (i.Status == InvoiceStatus.Pending || i.Status == InvoiceStatus.SAConfirmed)).Sum(i => i.Amount),
                        c.SalesId,
                        SalesManagerId = sales.SmId,
                        GiftCount = _context.Gifts.Count(g => g.ContractId == c.Id),
                        Discount = _context.Coupons.Where(cp => cp.ContractId == c.Id).Sum(cp => cp.Discount),
                        c.LeadId,
                        SourceName = s.Name,
                        Dos = sales != null && sales.DosId != null ? _context.Users.First(x => x.Id == sales.DosId).Name : string.Empty,
                        SmName = sales != null && sales.SmId != null ? _context.Users.First(x => x.Id == sales.SmId).Name : string.Empty,
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.ContractCode))
        {
            query = query.Where(c => c.ContractCode.ToLower().Contains(filterOptions.ContractCode.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(c => c.PhoneNumber.ToLower().Contains(filterOptions.PhoneNumber.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.IdentityNumber))
        {
            query = query.Where(c => c.IdentityNumber.ToLower().Contains(filterOptions.IdentityNumber.ToLower()));
        }
        if (filterOptions.FromDate.HasValue && filterOptions.ToDate.HasValue)
        {
            query = query.Where(c => c.CreatedDate.Date >= filterOptions.FromDate.Value.Date && c.CreatedDate.Date <= filterOptions.ToDate.Value.Date);
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.CustomerName))
        {
            query = query.Where(c => c.CustomerName != null && c.CustomerName.ToLower().Contains(filterOptions.CustomerName.ToLower()));
        }
        if (filterOptions.LeadId.HasValue)
        {
            query = query.Where(c => c.LeadId == filterOptions.LeadId);
        }
        if (_hcaService.IsUserInRole(RoleName.Sales))
        {
            query = query.Where(c => c.SalesId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.SalesManager))
        {
            query = query.Where(c => c.SalesManagerId == userId);
        }
        query = query.OrderByDescending(c => c.CreatedDate);
        return await ListResult<object>.Success(query, filterOptions);
    }
}

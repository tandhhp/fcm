
using Waffle.Core.Services.Leads.Args;
using Waffle.Core.Services.Leads.Filters;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService;

public interface ILeadService
{
    Task<TResult> AddAsync(LeadCreateArgs leadCreateArgs);
    Task<TResult> AllowedDuplicateAsync(Guid id);
    Task<IEnumerable<string?>> AllPhoneNumbersAsync();
    Task<TResult> CheckinAsync(LeadCheckinArgs args);
    Task<TResult<object>> DetailAsync(Guid id);
    Task<TResult<byte[]?>> ExportCheckinAsync(LeadCheckinListFilterOptions filterOptions);
    Task<Lead?> FindAsync(Guid leadId);
    Task<Lead?> FindByPhoneNumberAsync(string? phoneNumber);
    Task<ListResult<object>> GetCheckinListAsync(LeadCheckinListFilterOptions filterOptions);
    Task<ListResult<object>> GetWaitingListAsync(LeadWaittingListFilterOptions filterOptions);
    Task<TResult> RejectAsync(LeadRejectArgs args);
    Task<TResult> UpdateAsync(LeadUpdateArgs args);
    Task<TResult> UpdateFeedbackAsync(LeadUpdateFeedbackArgs args);
}

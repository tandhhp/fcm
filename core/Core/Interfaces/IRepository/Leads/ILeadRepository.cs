using Waffle.Core.Services.Leads.Args;
using Waffle.Core.Services.Leads.Filters;
using Waffle.Core.Services.Leads.Results;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Leads;

public interface ILeadRepository : IAsyncRepository<Lead>
{
    Task AddFeedbackAsync(LeadFeedback feedback);
    Task AddSubLeadsAsync(Guid id, List<SubLeadCreateArgs> subLeads);
    Task<IEnumerable<string?>> AllPhoneNumbersAsync();
    Task<ExistingLeadResult?> FindByIdentityNumberAsync(string identityNumber);
    Task<Lead?> FindByPhoneNumberAsync(string? phoneNumber);
    Task<ListResult<object>> GetCheckinListAsync(LeadCheckinListFilterOptions filterOptions);
    Task<List<LeadExportCheckinResult>> GetExportCheckinDataAsync(LeadCheckinListFilterOptions filterOptions);
    Task<LeadFeedback?> GetFeedbackAsync(Guid id);
    Task<object> GetSubLeadsAsync(Guid id);
    Task<ListResult<object>> GetWaitingListAsync(LeadWaittingListFilterOptions filterOptions);
    Task<LeadFeedback> SaveFeedbackAsync(Guid leadId, int? tableId);
    Task UpdateFeedbackAsync(Lead lead, LeadFeedback feedback);
    Task UpdateSubLeadsAsync(Guid id, List<SubLeadUpdateArgs> subLeads);
}

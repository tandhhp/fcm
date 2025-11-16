using Waffle.Core.Services.Contacts.Args;
using Waffle.Core.Services.Contacts.Filters;
using Waffle.Core.Services.Contacts.Models;
using Waffle.Entities.Contacts;
using Waffle.Models;
using Waffle.Models.Filters;

namespace Waffle.Core.Interfaces.IRepository;

public interface IContactRepository : IAsyncRepository<Contact>
{
    Task<ListResult<object>> DialedCallsAsync(ContactFilterOptions filterOptions);
    Task<ListResult<object>> GetBlacklistAsync(BlacklistFilterOptions filterOptions);
    Task<TResult<object>> GetTmrReportAsync();
    Task<List<Contact>> GetUnassignedContactsAsync(int numberOfContact, int sourceId);
    Task<ListResult<object>> GetUnassignedListAsync(UnassignedFilterOptions filterOptions);
    Task<bool> IsPhoneExistAsync(string phoneNumber);
    Task<ListResult<dynamic>> ListAsync(ContactFilterOptions filterOptions);
    Task<ListResult<object>> NeedConfirmsAsync(ContactFilterOptions filterOptions);
    void Update(Contact contact);
}

using Microsoft.AspNetCore.Identity;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Components;
using Waffle.Models.Params.Products;

namespace Waffle.Core.Interfaces.IService;

public interface IWorkService
{
    Task<IdentityResult> DeleteAsync(Guid id);
    Task<IdentityResult> ActiveAsync(Guid id);
    Task<IdentityResult> SaveTagAsync(Tag tag);
    Task<IdentityResult> SaveColumnAsync(Column item);
    Task<T?> GetAsync<T>(Guid? id);
    T? Get<T>(string? arguments);
    Task<IdentityResult> SaveContactFormAsync(ContactForm item);
    Task<IdentityResult> SaveRowAsync(Row row);
    Task<IdentityResult> ColumnAddAsync(Column column);
    Task<IdentityResult> NavbarSettingSaveAsync(Navbar args);
    Task<IEnumerable<Option>> TagListAsync(WorkFilterOptions filterOptions);
    Task<IdentityResult> ItemAddAsync(WorkItem args);
    Task<WorkContent?> FindAsync(Guid? id);
    Task<IEnumerable<Option>> GetListAsync(BasicFilterOptions filterOptions);
    Task<dynamic> ExportByCatalogAsync(Guid catalogId);
    Task<ListResult<WorkListItem>> GetWorkListItemChildAsync(WorkFilterOptions filterOptions);
    Task<IEnumerable<WorkContent>> GetWorkContentChildsAsync(Guid parentId);
    Task<IdentityResult> SaveAsync(WorkContent args);
    Task AddAsync(WorkContent workContent);
    Task AddItemAsync(Guid workId, Guid catalogId);
    Task<IdentityResult> SaveArgumentsAsync(Guid id, object args);
    Task<WorkContent?> GetSummaryAsync(Guid id);
    Task<IdentityResult> UpdateSummaryAsync(WorkContent args);
    Task<IdentityResult> ItemDeleteAsync(WorkItem args);
    Task<List<T>> GetListChildAsync<T>(Guid parentId);
    IEnumerable<T?> ListAsync<T>(List<string> list);
    Task<ListResult<WorkListItem>> ListBySettingIdAsync(Guid id);
    Task<object?> GetListUnuseAsync(BasicFilterOptions filterOptions);
    Task<IdentityResult> SaveProductImageAsync(SaveImageModel args);
    Task<IEnumerable<WorkListItem>> GetComponentsInColumnAsync(Guid workId);
    IAsyncEnumerable<Column> ListColumnAsync(Guid rowId);
    Task<IEnumerable<Guid>> ListChildIdAsync(Guid parentId);
    Task<object?> GetUnuseWorksAsync(SearchFilterOptions filterOptions);
}

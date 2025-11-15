using Microsoft.AspNetCore.Identity;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService;

public interface IFileService
{
    Task<int> CountAsync();
    Task<ListResult<FileContent>> ListAsync(FileFilterOptions filterOptions);
    Task<IdentityResult> UploadFromUrlAsync(string url);
    Task<FileContent?> FindAsync(Guid id);
    Task<decimal> GetTotalSizeAsync();
}

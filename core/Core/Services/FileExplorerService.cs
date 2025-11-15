using Microsoft.AspNetCore.Identity;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Services;

public class FileExplorerService : IFileService
{
    private readonly ApplicationDbContext _context;
    private readonly IFileRepository _fileRepository;

    public FileExplorerService(ApplicationDbContext context, IFileRepository fileContentRepository)
    {
        _context = context;
        _fileRepository = fileContentRepository;
    }

    public async Task<int> CountAsync() => await _fileRepository.CountAsync();

    public Task<FileContent?> FindAsync(Guid id) => _fileRepository.FindAsync(id);

    public Task<decimal> GetTotalSizeAsync() => _fileRepository.GetTotalSizeAsync();

    public async Task<ListResult<FileContent>> ListAsync(FileFilterOptions filterOptions)
    {
        var query = _context.FileContents
            .Where(x => (string.IsNullOrWhiteSpace(filterOptions.Name) || x.Name.ToLower().Contains(filterOptions.Name.ToLower())) && (string.IsNullOrEmpty(filterOptions.Type) || filterOptions.Type.Contains(x.Type)))
            .OrderByDescending(x => x.Id);
        return await ListResult<FileContent>.Success(query, filterOptions);
    }

    public async Task<IdentityResult> UploadFromUrlAsync(string url)
    {
        var uri = new Uri(url);
        await _context.FileContents.AddAsync(new FileContent
        {
            Name = Path.GetFileName(uri.LocalPath),
            Url = url,
            Size = 0,
            Type = Path.GetExtension(uri.LocalPath).ToLower()
        });
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }
}

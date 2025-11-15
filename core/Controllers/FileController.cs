using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Foundations;
using Waffle.Models;
using Waffle.Models.Args;

namespace Waffle.Controllers;

public class FileController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context, IFileService fileExplorerService, IHCAService _hcaService) : BaseController
{
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
    private readonly ApplicationDbContext _context = context;
    private readonly IFileService _fileService = fileExplorerService;

    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] FileFilterOptions filterOptions) => Ok(await _fileService.ListAsync(filterOptions));

    [HttpPost("delete-file-content/{id}")]
    public async Task<IActionResult> DeleteFileContentAsync([FromRoute] Guid id)
    {
        var fileContent = await _fileService.FindAsync(id);
        if (fileContent is null) return BadRequest("File not found!");
        var path = Path.Combine(_webHostEnvironment.WebRootPath, "files", fileContent.Name);
        if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        _context.FileContents.Remove(fileContent);
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadAsync([FromForm] FileUploadArgs args)
    {
        if (args is null || args.File is null) return BadRequest("File not found!");
        if (args.File.Length == 0) return BadRequest("File is empty!");
        if (args.File.Length > 10485760) return BadRequest("File size must be less than 10MB!");
        var file = args.File;
        var folderPath = _hcaService.GetUserId().ToString();
        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "files", folderPath);
        if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, file.FileName);

        using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }
        var url = $"{Request.Scheme}://{Request.Host}/files/{folderPath}/{file.FileName}";
        var fileC = new FileContent
        {
            Name = file.FileName,
            Size = file.Length,
            Type = file.ContentType,
            Url = url
        };
        await _context.FileContents.AddAsync(fileC);
        await _context.SaveChangesAsync();
        return Ok(TResult<object>.Ok(fileC));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id) => Ok(await _context.FileContents.FindAsync(id));

    [HttpGet("count")]
    public async Task<IActionResult> CountAsync() => Ok(await _fileService.CountAsync());

    [HttpGet("total-size")]
    public async Task<IActionResult> GetTotalSizeAsync() => Ok(await _fileService.GetTotalSizeAsync());
}

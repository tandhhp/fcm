using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Xml;
using Waffle.Data;
using Waffle.Extensions;
using Waffle.Foundations;

namespace Waffle.Controllers;

public class MetaController : BaseController
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MetaController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet("/info"), AllowAnonymous]
    public IActionResult Info()
    {
        var assembly = typeof(Program).Assembly;

        var creationDate = System.IO.File.GetCreationTime(assembly.Location);
        var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

        return Ok($"Version: {version}, Last Updated: {creationDate}");
    }

    [HttpGet("sitemap-generator"), AllowAnonymous]
    public async Task<IActionResult> SitemapGeneratorAsync()
    {
        var catalogs = await _context.Catalogs.Where(x => x.Active).ToListAsync();
        // Create an XmlWriter to generate the sitemap XML file
        var path = Path.Combine(_webHostEnvironment.WebRootPath, "sitemap.xml");
        using (XmlWriter writer = XmlWriter.Create(path))
        {
            // Start the document
            writer.WriteStartDocument();
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            // Add URLs and metadata
            foreach (var item in catalogs)
            {
                var url = item.GetUrl();
                AddUrl(writer, $"https://{Request.Host.Value}{url}", item.ModifiedDate.ToString("yyyy-MM-dd"), "0.8");
            }

            // End the document
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
        return Ok();
    }

    static void AddUrl(XmlWriter writer, string url, string lastModified, string priority)
    {
        writer.WriteStartElement("url");
        writer.WriteElementString("loc", url);
        writer.WriteElementString("lastmod", lastModified);
        writer.WriteElementString("priority", priority);
        writer.WriteEndElement();
    }
}

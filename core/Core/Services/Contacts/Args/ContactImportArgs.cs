namespace Waffle.Core.Services.Contacts.Args;

public class ContactImportArgs
{
    public int SourceId { get; set; }
    public IFormFile? File { get; set; }
}

using Waffle.Models;

namespace Waffle.Core.Services.Contacts.Args;

public class ContactTransferFilterOptions : FilterOptions
{
    public int? GroupId { get; set; }
    public int? SourceId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
    public Guid? TelesalesId { get; set; }
}

public class ContactTransferDestinationArgs
{
    public int? GroupId { get; set; }
    public int? SourceId { get; set; }
    public int? TeamId { get; set; }
    public Guid? TelesalesId { get; set; }
}

public class ContactTransferBySearchArgs
{
    public ContactTransferFilterOptions Filter { get; set; } = new();
    public ContactTransferDestinationArgs Destination { get; set; } = new();
}

public class ContactTransferByCaseArgs
{
    public List<Guid> ContactIds { get; set; } = new();
    public ContactTransferDestinationArgs Destination { get; set; } = new();
}

public class ContactTransferByFileArgs
{
    public IFormFile? File { get; set; }
    public int? GroupId { get; set; }
    public int? SourceId { get; set; }
    public int? TeamId { get; set; }
    public Guid? TelesalesId { get; set; }
}
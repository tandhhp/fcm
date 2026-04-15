using Waffle.Models;

namespace Waffle.Core.Services.Contacts.Args;

public class ContactRevokeSourceFilterOptions : FilterOptions
{
    public int? GroupId { get; set; }
    public int? TeamId { get; set; }
    public int? SourceId { get; set; }
    public Guid? TelesalesId { get; set; }
}

public class ContactRevokeSourceByCaseArgs
{
    public List<Guid> ContactIds { get; set; } = new();
}

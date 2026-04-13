using Waffle.Entities;
using Waffle.Entities.Contacts;

namespace Waffle.Core.Services.Contacts.Results;

public class ContactListItem : BaseEntity
{
    public string? PhoneNumber { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? Name { get; set; }
    public string? Note { get; set; }
    public Guid? TelesalesId { get; set; }
    public string? TelesalesName { get; set; }
    public bool ShowUp { get; set; }
    public int? SourceId { get; set; }
    public Guid? TmId { get; set; }
    public Guid? DotId { get; set; }
    public Guid? DosId { get; set; }
    public bool Confirm1 { get; set; }
    public string? SourceName { get; set; }
    public int? TypeOfDataId { get; set; }
    public string? TypeOfDataName { get; set; }
    public SourceType? SourceType { get; set; }
    public bool Called { get; set; }
    public string? Name2 { get; set; }
    public string? PhoneNumber2 { get; set; }
}

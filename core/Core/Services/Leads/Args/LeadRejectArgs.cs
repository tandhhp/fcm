using Waffle.Entities;

namespace Waffle.Core.Services.Leads.Args;

public class LeadRejectArgs : BaseEntity
{
    public string? Note { get; set; }
}

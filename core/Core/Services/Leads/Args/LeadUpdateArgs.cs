using Waffle.Entities;

namespace Waffle.Core.Services.Leads.Args;

public class LeadUpdateArgs : BaseEntity
{
    public string PhoneNumber { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Email { get; set; }
    public bool? Gender { get; set; }
    public Guid? TelesalesId { get; set; }
    public Guid EventId { get; set; }
    public DateTime EventDate { get; set; }
    public string? Note { get; set; }
    public string? Address { get; set; }
    public int BranchId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Guid CreatedBy { get; set; }
    public string IdentityNumber { get; set; } = default!;
    public List<SubLeadUpdateArgs>? SubLeads { get; set; }
}

public class SubLeadUpdateArgs
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string? IdentityNumber { get; set; }
    public bool? Gender { get; set; }
}
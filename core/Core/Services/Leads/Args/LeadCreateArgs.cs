namespace Waffle.Core.Services.Leads.Args;

public class LeadCreateArgs
{
    public string PhoneNumber { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Email { get; set; }
    public bool? Gender { get; set; }
    public Guid? TelesalesId { get; set; }
    public Guid EventId { get; set; }
    public DateTime EventDate { get; set; }
    public string? Note { get; set; }
    public string? IdentityNumber { get; set; }
    public string? Address { get; set; }
    public int BranchId { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public List<SubLeadCreateArgs>? SubLeads { get; set; }
}

public class SubLeadCreateArgs
{
    public string PhoneNumber { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? IdentityNumber { get; set; }
    public bool? Gender { get; set; }
}

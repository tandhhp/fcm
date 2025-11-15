namespace Waffle.Core.Services.Leads.Args;

public class LeadCheckinArgs
{
    public Guid LeadId { get; set; }
    public string IdentityNumber { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public string? Note { get; set; }
    public bool? Gender { get; set; }
    public string Name { get; set; } = default!;
    public int? TableId { get; set; }
    public int? AttendanceId { get; set; }

    public List<SubLeadUpdateArgs>? SubLeads { get; set; }
}

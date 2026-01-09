namespace Waffle.Core.Services.Leads.Results;

public class LeadCustomer
{
    public Guid Id { get; set; }
    public string? IdentityNumber { get; set; }
    public string Name { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Note { get; set; }
    public int Count { get; set; }
    public bool Duplicated { get; set; }
    public IEnumerable<SubLeadCustomer> SubLeads { get; set; } = [];
}

public class SubLeadCustomer
{
    public Guid Id { get; set; }
    public string? IdentityNumber { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? Gender { get; set; }
}
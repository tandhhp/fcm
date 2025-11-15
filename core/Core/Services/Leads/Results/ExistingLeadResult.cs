namespace Waffle.Core.Services.Leads.Results;

public class ExistingLeadResult
{
    public DateTime EventDate { get; set; }
    public string EventName { get; set; } = default!;
    public Guid Id { get; set; }
    public bool Dupplicated { get; set; }
}

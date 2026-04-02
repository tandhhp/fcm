using System.ComponentModel.DataAnnotations;

namespace Waffle.Entities.Contacts;

public class TypeOfData : BaseEntity<int>
{
    [StringLength(256)]
    public string Name { get; set; } = default!;
    public SourceType Source { get; set; }

    public ICollection<Source>? Sources { get; set; }
}

public enum SourceType
{
    [Display(Name = "Cold Data")]
    ColdData = 1,
    [Display(Name = "Company")]
    Company = 2,
    [Display(Name = "Private")]
    Private = 3,
    [Display(Name = "Reference")]
    Reference = 4
}

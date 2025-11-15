namespace Waffle.Models.Args.Catalogs;

public class GetComponentsArgs
{
    public string NormalizedName { get; set; } = default!;
    public string Locale { get; set; } = default!;
}

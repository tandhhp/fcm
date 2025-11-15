namespace Waffle.Core.Interfaces;

public interface IComponent
{
    Guid Id { get; set; }
    string NormalizedName { get; set; }
}

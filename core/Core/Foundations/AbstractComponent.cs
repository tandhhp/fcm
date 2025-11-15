using System.Text.Json.Serialization;
using Waffle.Core.Interfaces;
using Waffle.Entities;

namespace Waffle.Core.Foundations;

public abstract class AbstractComponent : BaseEntity, IComponent
{
    [JsonIgnore]
    public string NormalizedName { get; set; } = default!;
}

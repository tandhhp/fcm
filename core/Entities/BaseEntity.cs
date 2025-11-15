using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Waffle.Entities;

public abstract class BaseEntity
{
    [Key, JsonPropertyName("id")]
    public Guid Id { get; set; }
}

public class BaseEntity<T>
{
    [Key, JsonPropertyName("id")]
    public T Id { get; set; } = default!;
}

public class AuditEntity : BaseEntity
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

public class AuditEntity<T> : BaseEntity<T>
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
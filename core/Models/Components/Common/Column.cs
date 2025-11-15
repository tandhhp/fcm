using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

[Display(Name = "Column", GroupName = GroupName.Grid, Prompt = "column", AutoGenerateFilter = true)]
public class Column : AbstractComponent
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("className")]
    public string ClassName { get; set; } = "col";
    [JsonPropertyName("rowId")]
    public Guid RowId { get; set; }

    [JsonPropertyName("items")]
    public IEnumerable<WorkListItem> Items { get; set; } = new List<WorkListItem>();
}

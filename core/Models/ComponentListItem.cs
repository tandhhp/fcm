using Waffle.Entities;

namespace Waffle.Models
{
    public class ComponentListItem : BaseEntity
    {
        public ComponentListItem()
        {
            Name = string.Empty;
        }
        public string Name { get; set; }
    }
}

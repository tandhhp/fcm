using Waffle.Entities;

namespace Waffle.Models.ViewModels
{
    public class DropModel
    {
        public Guid DragNodeKey { get; set; }
        public Guid Node { get; set; }
        public bool DropToGap { get; set; }
    }

    public class TagListItem : Catalog
    {
        public int PostCount { get; set; }
    }
}

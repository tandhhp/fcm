namespace Waffle.Models.Components
{
    public class Tree
    {
        public Tree()
        {
            Title = string.Empty;
            Icon = string.Empty;
        }
        public Guid Key { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public List<Tree>? Children { get; set; }
    }
}

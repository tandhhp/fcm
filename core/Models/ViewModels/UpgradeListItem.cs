namespace Waffle.Models.ViewModels
{
    public class UpgradeListItem
    {
        public UpgradeListItem()
        {
            Name = string.Empty;
            Url = string.Empty;
        }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}

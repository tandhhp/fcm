using Waffle.Entities;

namespace Waffle.Models.Args
{
    public class ApproveTopupArgs
    {
        public Guid Id { get; set; }
        public TopupStatus Status { get; set; }
        public string? Note { get; set; }
    }
}

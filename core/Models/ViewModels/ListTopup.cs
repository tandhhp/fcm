using Waffle.Entities;

namespace Waffle.Models.ViewModels
{
    public class ListTopup
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? AccountantApprovedDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DirectorApprovedDate { get; set; }
        public TopupStatus Status { get; set; }
        public string? Note { get; set; }
        public string? CardHolderName { get; set; }
        public string? AccountantName { get; set; }
        public string? DirectorName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public TopupType Type { get; set; }
        public int BranchId { get; set; }
    }
}

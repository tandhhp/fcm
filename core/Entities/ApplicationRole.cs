using Microsoft.AspNetCore.Identity;

namespace Waffle.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
    }
}

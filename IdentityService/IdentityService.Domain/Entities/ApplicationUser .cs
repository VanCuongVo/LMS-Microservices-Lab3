using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;


    }
}

using Microsoft.AspNetCore.Identity;

namespace ApiProjectPRN231.Models
{
    public class UserApp: IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
    }
}

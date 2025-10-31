using Microsoft.AspNetCore.Identity;

namespace TaskManagerAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}

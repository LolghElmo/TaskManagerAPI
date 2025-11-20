using Microsoft.AspNetCore.Identity;


namespace TaskManagerAPI.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public ICollection<TaskItem>? Tasks { get; set; }
    }
}

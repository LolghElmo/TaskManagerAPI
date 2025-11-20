
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Core.Models;

namespace TaskManagerAPI.Infrastructure.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks { get; set; }

        public DbSet<ApplicationUser> User { get; set; }
    }
}

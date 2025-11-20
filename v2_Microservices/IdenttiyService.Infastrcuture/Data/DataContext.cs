using IdenttiyService.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdenttiyService.Infastrcuture.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions) { }
        public DbSet<User> Users { get; set; }
    }
}

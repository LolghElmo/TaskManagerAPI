using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(DataContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }
            if (_context.Roles.Any(x => x.Name == Utility.Helper.Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(Utility.Helper.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Utility.Helper.TaskFinisher)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Utility.Helper.TaskOrganizer)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Utility.Helper.User)).GetAwaiter().GetResult();
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "Admin User"
            }, "!GGmo@123456@gg").GetAwaiter().GetResult();
            ApplicationUser user = _context.Users.FirstOrDefault(static x => x.Email == "admin@gmail.com");
            _userManager.AddToRoleAsync(user, Utility.Helper.Admin).GetAwaiter().GetResult();
        }
    }
}

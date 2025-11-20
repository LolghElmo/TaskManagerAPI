using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerAPI.Core.Interfaces;
using TaskManagerAPI.Core.Models;

namespace TaskManagerAPI.Infrastructure.Data
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
            if (_context.Roles.Any(x => x.Name == GlobalStrings.Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(GlobalStrings.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(GlobalStrings.TaskFinisher)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(GlobalStrings.TaskOrganizer)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(GlobalStrings.User)).GetAwaiter().GetResult();
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "Admin User"
            }, "!GGmo@123456@gg").GetAwaiter().GetResult();
            ApplicationUser user = _context.Users.FirstOrDefault(static x => x.Email == "admin@gmail.com");
            _userManager.AddToRoleAsync(user, GlobalStrings.Admin).GetAwaiter().GetResult();
        }
    }

}

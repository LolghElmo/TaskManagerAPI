using IdentityService.Domain.Interfaces;
using IdentityService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace IdentityService.Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(
            DataContext context,
            UserManager<User> userManager,
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
                // Apply pending migrations if any
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not apply migrations: {ex.Message}");
                return;
            }
            // Create Roles if they do not exist
            if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole("User")).GetAwaiter().GetResult();
            }

            if (_userManager.FindByEmailAsync("ghaithomo@gmail.com").GetAwaiter().GetResult() == null)
            {
                // Create Admin User
                var adminUser = new User
                {
                    UserName = "testmail@gmail.com", 
                    Email = "testmail@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Ghaith", 
                    LastName = "Admin"   
                };

                var result = _userManager.CreateAsync(adminUser, "!test@123456").GetAwaiter().GetResult();

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
                }
            }


        }
    }
}
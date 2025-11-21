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
                    UserName = "ghaithomo@gmail.com", 
                    Email = "ghaithomo@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Ghaith", 
                    LastName = "Admin"   
                };

                var result = _userManager.CreateAsync(adminUser, "!GGmo@123456@gg").GetAwaiter().GetResult();

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
                }
            }

            if (_userManager.FindByEmailAsync("ghaitho.mo@gmail.com").GetAwaiter().GetResult() == null)
            {
                // Create Regular User
                var regularUser = new User
                {
                    UserName = "ghaitho.mo@gmail.com",
                    Email = "ghaitho.mo@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Ghaith",
                    LastName = "User"
                };

                _userManager.CreateAsync(regularUser, "!GGmo@123456@gg").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(regularUser, "User").GetAwaiter().GetResult();
            }
        }
    }
}
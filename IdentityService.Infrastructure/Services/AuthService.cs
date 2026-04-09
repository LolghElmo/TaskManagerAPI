using Common.Models;
using IdentityService.Domain.Interfaces;
using IdentityService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService
            (
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<Result<(User User, string Token)>> LoginAsync(string identifier, string password)
        {
            // Try to find user by email
            var user = await _userManager.FindByEmailAsync(identifier);

            // If not found by email, try username
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(identifier);
            }

            if (user == null) return Result<(User, string)>.Failure("Invalid login attempt.");

            // Check password
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) return Result<(User, string)>.Failure("Invalid login attempt.");

            // Generate JWT token and return
            var token = _tokenService.GenerateToken(user);

            return Result<(User, string)>.Success((user, token));
        }

        public async Task<Result<User>> RegisterAsync(string email, string username, string password, string firstName, string lastName)
        {
            // Check if Email exists
            if (await _userManager.FindByEmailAsync(email) != null)
            {
                return Result<User>.Failure("User with this email already exists.");
            }

            // Check if Username exists
            if (await _userManager.FindByNameAsync(username) != null)
            {
                return Result<User>.Failure("Username is already taken.");
            }

            // Create new user object 
            var user = new User
            {
                UserName = username,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                EmailConfirmed = true // No Email Confirmation for simplicity
            };

            // Create user
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<User>.Failure(errors);
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, "User");

            return Result<User>.Success(user);
        }
    }
}

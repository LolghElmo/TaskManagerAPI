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

        public async Task<Result<string>> LoginAsync(string email, string password)
        {
            // Find the user by email
            var user = await _userManager.FindByNameAsync(email);
            if (user == null) return Result<string>.Failure("Invalid login attempt.");

            // Check the password
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) return Result<string>.Failure("Invalid login attempt.");

            // Generate JWT token
            var token = _tokenService.GenerateToken(user);

            // Return the token
            return Result<string>.Success(token);

        }

        public async Task<Result<bool>> RegisterAsync(string email, string password, string firstName, string lastName)
        {
            // Check if user already exists
            if (await _userManager.FindByEmailAsync(email) != null)
            {
                return Result<bool>.Failure("User already exists.");
            }

            var user = new User
            {
                UserName = email,
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
                return Result<bool>.Failure(errors);
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, "User");

            return Result<bool>.Success(true);
        }
    }
}

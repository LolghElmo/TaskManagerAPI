using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TaskManagerAPI.Application.Models.DTOs.User;
using TaskManagerAPI.Core.Interfaces;
using TaskManagerAPI.Core.Models;

namespace TaskManagerAPI.Application.Features.Account
{
    public class LoginUserCommand : IRequest<LoginResponseDto>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginUserCommandHandler> _logger;

        public LoginUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            ILogger<LoginUserCommandHandler> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<LoginResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // Find user by email
            var user = await _userManager.FindByNameAsync(request.Email);

            if (user == null)
            {
                // Log failed login attempt
                _logger.LogWarning("Failed login attempt for email: {Email}", request.Email);
                throw new Exception("Invalid Email, username or password.");
            }
            // Check password
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
            {
                // Log failed login attempt
                _logger.LogWarning("Failed login attempt (wrong password) for email: {Email}", request.Email);
                throw new Exception("Invalid Email, username or password.");
            }

            // Generate token
            var token = _tokenService.CreateToken(user);
            if (token == null)
            {
                // Log token generation failure
                _logger.LogCritical("Token generation failed for user: {Username}", user.UserName);

                throw new Exception("Error generating token.");
            }
            // Log successful login
            _logger.LogInformation("User {Username} logged in successfully. Email: {email}", user.UserName, user.Email);

            // Return response
            return new LoginResponseDto
            {
                Message = "Login successful.",
                Username = user.UserName,
                Token = token
            };
        }
    }
}
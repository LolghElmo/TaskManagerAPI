using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TaskManagerAPI.Application.Models.DTOs.User;
using TaskManagerAPI.Core.Models;

namespace TaskManagerAPI.Application.Features.Account
{
    public class RegisterUserCommand : IRequest<UserDto>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<RegisterUserCommandHandler> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Check if user with the same email already exists
            if (await _userManager.FindByNameAsync(request.Email) != null)
            {
                // Log warning about existing user
                _logger.LogWarning("Registration attempt with existing email: {Email}", request.Email);
                throw new Exception("User with this email already exists.");
            }

            // Create new user
            var user = _mapper.Map<ApplicationUser>(request);
            user.UserName = request.Email;

            // Create user in the identity store
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                // Log failure details
                _logger.LogError("User creation failed for email: {Email}.", request.Email);
                throw new Exception("Error creating user.");
            }

            // Assign role to the user
            await _userManager.AddToRoleAsync(user, request.Role);

            // Log successful registration
            _logger.LogInformation("User {Username} registered successfully with role {Role}. Email: {Email}", user.UserName, request.Role, user.Email);

            // Map to UserDto and return
            return _mapper.Map<UserDto>(user);
        }
    }
}
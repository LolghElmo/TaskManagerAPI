using AutoMapper;
using Common.Models;
using IdentityService.Application.DTOs.Auth;
using IdentityService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Application.Features.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<UserDto>>
    {
        private readonly IAuthService _authService;
        private readonly ILogger<RegisterCommandHandler> _logger;
        private readonly IMapper _mapper;
        public RegisterCommandHandler(IAuthService authService, ILogger<RegisterCommandHandler> logger, IMapper mappers)
        {
            _authService = authService;
            _logger = logger;
            _mapper = mappers;
        }


        public async Task<Result<UserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var req = request.RegisterRequest;

            // Attempt to register the user
            var result = await _authService.RegisterAsync(
                        req.Email,
                        req.Username,
                        req.Password,
                        req.FirstName,
                        req.LastName);

            // Check if registration was successful
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Registration failed for {Email}: {Error}", req.Email, result.Error);
                return Result<UserDto>.Failure(result.Error ?? "Registration failed");
            }

            // Map the registered user to UserDto
            var userDto = _mapper.Map<UserDto>(result.Value);

            _logger.LogInformation("User {Id} registered successfully.", userDto.Id);

            // Return success result with UserDto
            return Result<UserDto>.Success(userDto);
        }
    }
}

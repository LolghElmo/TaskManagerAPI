using AutoMapper;
using Common.Models;
using IdentityService.Application.DTOs.Auth;
using IdentityService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Application.Features.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
    {
        private readonly IAuthService _authService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public LoginCommandHandler(IAuthService authService, ILogger logger, IMapper mapper)
        {
            _authService = authService;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var req = request.LoginRequest;

            // Attempt to register the user
            var result = await _authService.LoginAsync(req.Email, req.Password);

            // Check if registration was successful
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Login failed for {Email}: {Error}", req.Email, result.Error);
                return Result<LoginResponseDto>.Failure(result.Error ?? "Login failed");
            }

            // Map the user and token to the response DTO
            var (user, token) = result.Value;
            var userDto = _mapper.Map<UserDto>(user);
            var response = new LoginResponseDto
            {
                Token = token,
                User = userDto
            };

            _logger.LogInformation("User {Id} Logged-in successfully.", userDto.Id);

            // Return success result with LoginResponseDto
            return Result<LoginResponseDto>.Success(response);
        }
    }
}

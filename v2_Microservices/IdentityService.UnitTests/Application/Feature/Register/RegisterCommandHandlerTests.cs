using AutoMapper;
using Common.Models;
using FluentAssertions;
using IdentityService.Application.DTOs.Auth;
using IdentityService.Application.Features.Register;
using IdentityService.Domain.Interfaces;
using IdentityService.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace IdentityService.UnitTests.Application.Features.Register
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<RegisterCommandHandler>> _loggerMock;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RegisterCommandHandler>>();

            _handler = new RegisterCommandHandler(
                _authServiceMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenRegistrationSucceeds()
        {
            // Arrange
            var requestDto = new RegisterRequestDto
            {
                Email = "new@test.com",
                Password = "Pass",
                FirstName = "New",
                LastName = "User",
                Username = "newuser"
            };
            var command = new RegisterCommand { RegisterRequest = requestDto };

            var createdUser = new User { Id = "123", Email = requestDto.Email, UserName = requestDto.Username, FirstName = requestDto.FirstName, LastName = requestDto.LastName };

            // Mock Service Success
            _authServiceMock.Setup(x => x.RegisterAsync(
                requestDto.Email, requestDto.Username, requestDto.Password, requestDto.FirstName, requestDto.LastName))
                .ReturnsAsync(Result<User>.Success(createdUser));

            // Mock Mapper
            var userDto = new UserDto { UserName="C",Id = "123", Email = requestDto.Email, FirstName = requestDto.FirstName, LastName = requestDto.LastName };
            _mapperMock.Setup(x => x.Map<UserDto>(createdUser)).Returns(userDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().Be("123");
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenEmailAlreadyExists()
        {
            // Arrange
            var command = new RegisterCommand
            {
                RegisterRequest = new RegisterRequestDto
                {
                    Email = "exists@test.com",
                    Password = "Pass",
                    FirstName = "A",
                    LastName = "B",
                    Username = "taken"
                }
            };

            // Mock Service Failure
            _authServiceMock.Setup(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(Result<User>.Failure("User already exists"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("User already exists");
        }
    }
}
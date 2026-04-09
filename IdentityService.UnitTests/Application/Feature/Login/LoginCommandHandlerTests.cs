using AutoMapper;
using Common.Models;
using FluentAssertions;
using IdentityService.Application.DTOs.Auth;
using IdentityService.Application.Features.Login;
using IdentityService.Domain.Interfaces;
using IdentityService.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace IdentityService.UnitTests.Application.Features.Login
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<LoginCommandHandler>> _loggerMock;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            // 1. Create the fake objects (Mocks)
            _authServiceMock = new Mock<IAuthService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<LoginCommandHandler>>();

            // 2. Inject them into the Handler
            _handler = new LoginCommandHandler(
                _authServiceMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCredentialsAreValid()
        {
            // Arrange
            var command = new LoginCommand
            {
                LoginRequest = new LoginRequestDto { Username="C",Email = "a@b.com", Password = "pass" }
            };

            var user = new User { FirstName = "A", LastName = "B", Email = "a@b.com" };
            var token = "fake-jwt-token";

            // Tell the Mock: "When LoginAsync is called, return Success(user, token)"
            _authServiceMock.Setup(x => x.LoginAsync(command.LoginRequest.Email, command.LoginRequest.Password))
                .ReturnsAsync(Result<(User, string)>.Success((user, token)));

            // Tell the Mapper: "When asking for UserDto, return this specific object"
            var expectedUserDto = new UserDto { Id = "1", UserName="C",Email = "a@b.com", FirstName = "A", LastName = "B" };
            _mapperMock.Setup(x => x.Map<UserDto>(user))
                .Returns(expectedUserDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Token.Should().Be(token);
            result.Value.User.Should().Be(expectedUserDto); 
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenServiceFails()
        {
            // Arrange
            var command = new LoginCommand
            {
                LoginRequest = new LoginRequestDto { Username="c",Email = "wrong@b.com", Password = "wrong" }
            };

            // Tell the Mock: "Return Failure"
            _authServiceMock.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(Result<(User, string)>.Failure("Invalid login"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Invalid login");
        }
    }
}
using IdentityService.Application.DTOs.Auth;
using IdentityService.Application.Features.Login;
using IdentityService.Application.Features.Register;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.IntegrationTests
{
    public class AuthTest : BaseIntegrationTest
    {
        public AuthTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }
        [Fact]
        public async Task RegisterAccountTest()
        {
            // Arrange
            var command = new RegisterCommand
            {
                RegisterRequest = new RegisterRequestDto
                {
                    Email = "test@example.com",
                    Username = "testuser",
                    Password = "Test123!",
                    FirstName = "Test",
                    LastName = "User"
                }
            };

            // Act
            var result = await Sender.Send(command);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
        }
        [Fact]
        public async Task LoginAccountTest()
        {
            // Arrange 
            var registerCommand = new RegisterCommand
            {
                RegisterRequest = new RegisterRequestDto
                {
                    Email = "login-test@example.com",
                    Username = "loginuser",
                    Password = "Test123!",
                    FirstName = "Login",
                    LastName = "User"
                }
            };
            await Sender.Send(registerCommand);

            var loginCommand = new LoginCommand
            {
                LoginRequest = new LoginRequestDto
                {
                    Identifier = "login-test@example.com",
                    Password = "Test123!"
                }
            };

            // Act
            var result = await Sender.Send(loginCommand);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);   
            Assert.False(string.IsNullOrEmpty(result.Value.Token)); 
        }
        [Fact]
        public async Task Login_WithNonExistentUser_Fails()
        {
            // Arrange
            var loginCommand = new LoginCommand
            {
                LoginRequest = new LoginRequestDto
                {
                    Identifier = "doesnotexist@example.com",
                    Password = "Test123!"
                }
            };
            // Act
            var result = await Sender.Send(loginCommand);

            // Asset
            Assert.False(result.IsSuccess);
        }

    }
}

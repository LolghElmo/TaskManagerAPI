using FluentAssertions;
using IdentityService.Domain.Models;
using IdentityService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace IdentityService.UnitTests.Infrastructure.Services
{
    public class TokenServiceTests
    {
        [Fact]
        public void GenerateToken_ShouldReturnTokenString_WhenConfigIsContext()
        {
            // Arrange
            var configMock = new Mock<IConfiguration>();

            configMock.Setup(c => c["JwtSettings:SecretKey"]).Returns("super_secret_key_must_be_long_enough_for_security");
            configMock.Setup(c => c["JwtSettings:Issuer"]).Returns("http://localhost");
            configMock.Setup(c => c["JwtSettings:Audience"]).Returns("http://localhost");

            var tokenService = new TokenService(configMock.Object);
            var user = new User { Id = "guid", Email = "test@test.com", FirstName = "Test", LastName = "User" };

            // Act
            var result = tokenService.GenerateToken(user);

            // Assert
            // DEVELOPERS NOTE: In a real test we might want to validate the token structure or claims
            result.Should().NotBeNullOrEmpty();
        }
    }
}
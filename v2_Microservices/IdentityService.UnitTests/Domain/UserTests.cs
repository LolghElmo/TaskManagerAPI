using FluentAssertions;
using IdentityService.Domain.Models;
using Xunit;

namespace IdentityService.UnitTests.Domain
{
    public class UserTests
    {
        [Fact]
        public void FullName_ShouldCombineFirstAndLastName_WhenCalled()
        {
            // Arrange
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "test@test.com",
                UserName = "john.doe"
            };

            // Act
            var fullName = user.FullName;

            // Assert
            fullName.Should().Be("John Doe");
        }
    }
}
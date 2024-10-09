using Moq;
using TinyUrl.AuthenticationService.Bussiness.Services;
using TinyUrl.AuthenticationService.Infrastructure.Common;
using TinyUrl.AuthenticationService.Infrastructure.Entities;
using TinyUrl.AuthenticationService.Infrastructure.Exceptions;
using TinyUrl.AuthenticationService.Infrastructure.Repositories;

namespace TinyUrl.AuthenticationService.UnitTests.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_UserExists_ReturnsUserContract()
        {
            // Arrange
            var userId = 1;
            var user = new User // Assuming this is your user model
            {
                Id = userId,
                Username = "testuser",
                Email = "test@example.com",
                Password = "hashedpassword"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username); // Assuming UserMapping.ToContract maps the Username
            Assert.Equal(user.Email, result.Email); // Assuming UserMapping.ToContract maps the Email
        }

        [Fact]
        public async Task GetUserByIdAsync_UserDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var userId = 1;

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync((User)null); // Simulating a null response

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _userService.GetUserByIdAsync(userId));

            Assert.Equal(ErrorMessages.UserNotFoundErrorMessage, exception.Message);
        }
    }
}

using Moq;
using TinyUrl.AuthenticationService.Infrastructure.Common;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Requests;
using TinyUrl.AuthenticationService.Infrastructure.Entities;
using TinyUrl.AuthenticationService.Infrastructure.Exceptions;
using TinyUrl.AuthenticationService.Infrastructure.Repositories;

namespace TinyUrl.AuthenticationService.UnitTests.Tests
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Bussiness.Services.AuthenticationService  _authenticationService;

        public AuthenticationServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authenticationService = new Bussiness.Services.AuthenticationService(_userRepositoryMock.Object);
        }

        // Test for LoginAsync - User not found or password incorrect
        [Fact]
        public async Task LoginAsync_UserNotFoundOrIncorrectPassword_ThrowsUnauthorizedException()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "test@example.com", Password = "wrongpassword" };

            _userRepositoryMock
                .Setup(repo => repo.GetUserByEmailAsync(loginRequest.Email))
                .ReturnsAsync((User?)null); // Simulate user not found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _authenticationService.LoginAsync(loginRequest));
            Assert.Equal(ErrorMessages.LoginFailedErrorMessage, exception.Message); // Ensure error message is as expected

            _userRepositoryMock.Verify(repo => repo.GetUserByEmailAsync(loginRequest.Email), Times.Once);
        }

        // Test for LoginAsync - Successful login
        [Fact]
        public async Task LoginAsync_Success_ReturnsTokenContract()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "test@example.com", Password = "password123" };

            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password123") // Correct hashed password
            };

            _userRepositoryMock
                .Setup(repo => repo.GetUserByEmailAsync(loginRequest.Email))
                .ReturnsAsync(user); // Simulate user found

            // Act
            var result = await _authenticationService.LoginAsync(loginRequest);

            // Assert
            Assert.NotNull(result.AccessToken);

            _userRepositoryMock.Verify(repo => repo.GetUserByEmailAsync(loginRequest.Email), Times.Once);
        }

        // Test for RegisterUserAsync - User already exists
        [Fact]
        public async Task RegisterUserAsync_UserAlreadyExists_ThrowsConflictException()
        {
            // Arrange
            var registerRequest = new RegisterUserRequest
            {
                Username = "existinguser",
                Email = "existing@example.com",
                Password = "password123"
            };

            _userRepositoryMock
                .Setup(repo => repo.CheckIfUserExistsAsync(registerRequest.Username, registerRequest.Email))
                .ReturnsAsync(true); // Simulate user already exists

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ConflictException>(() => _authenticationService.RegisterUserAsync(registerRequest));
            Assert.Equal(ErrorMessages.UserAlreadyExistsErrorMessage, exception.Message);

            _userRepositoryMock.Verify(repo => repo.CheckIfUserExistsAsync(registerRequest.Username, registerRequest.Email), Times.Once);
            _userRepositoryMock.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Never); // Ensure CreateUserAsync is not called
        }

        // Test for RegisterUserAsync - Successful registration
        [Fact]
        public async Task RegisterUserAsync_NewUser_CreatesUserSuccessfully()
        {
            // Arrange
            var registerRequest = new RegisterUserRequest
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "password123"
            };

            _userRepositoryMock
                .Setup(repo => repo.CheckIfUserExistsAsync(registerRequest.Username, registerRequest.Email))
                .ReturnsAsync(false); // Simulate user does not exist

            _userRepositoryMock
                .Setup(repo => repo.CreateUserAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask); // Simulate user creation

            // Act
            await _authenticationService.RegisterUserAsync(registerRequest);

            // Assert
            _userRepositoryMock.Verify(repo => repo.CreateUserAsync(It.Is<User>(user =>
                user.Username == registerRequest.Username &&
                user.Email == registerRequest.Email &&
                BCrypt.Net.BCrypt.Verify(registerRequest.Password, user.Password) // Ensure the password is hashed
            )), Times.Once);
        }
    }
}

using Fabricam.Web.Core.Models;
using Fabricam.Web.Core.Repositories;
using Fabricam.Web.Core.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Web.Core.Services.Tests
{
    public class AuthenticationService_Tests
    {
        // Values aren't important as these tests don't validate the properties
        private const string SOME_EMAIL = "some@em.ail";
        private const string SOME_PASSWORD = "somepassword";

        [Fact]
        public async Task ValidationFail_FailsLogin()
        {
            // Arrange
            AccountLoginModel model = new AccountLoginModel {
                Email = SOME_EMAIL,
                Password = SOME_PASSWORD
            };
            UserLoginResponse userRepoResult = new UserLoginResponse {
                Errors = new List<ValidationError> {
                    new ValidationError {Message = "some error"}
                }
            };

            // Setup mock
            Mock<IUserRepository> userRepositoryMock = SetupUserRepositoryMock(userRepoResult);

            // Act
            AuthenticateService service = new AuthenticateService(userRepositoryMock.Object);
            AuthenticateResult results = await service.AuthenticateUser(model);

            // Assert
            results.Success.Should().Be(false);
        }

        [Fact]
        public async Task InvalidUser_FailsLogin()
        {
            // Arrange
            AccountLoginModel model = new AccountLoginModel {
                Email = SOME_EMAIL,
                Password = SOME_PASSWORD
            };
            UserLoginResponse userRepoResult = new UserLoginResponse {
                UserIsValid = false
            };

            // Setup mock
            Mock<IUserRepository> userRepositoryMock = SetupUserRepositoryMock(userRepoResult);

            // Act
            AuthenticateService service = new AuthenticateService(userRepositoryMock.Object);
            AuthenticateResult results = await service.AuthenticateUser(model);

            // Assert
            results.Identity.Should().Be(null);
        }

        [Fact]
        public async Task ValidUser_SucceedsLogin()
        {
            // Arrange
            AccountLoginModel model = new AccountLoginModel {
                Email = SOME_EMAIL,
                Password = SOME_PASSWORD
            };
            UserLoginResponse userRepoResult = new UserLoginResponse {
                UserIsValid = true,
                UserId = 42 // greater than zero
            };

            // Setup mock
            Mock<IUserRepository> userRepositoryMock = SetupUserRepositoryMock(userRepoResult);

            // Act
            AuthenticateService service = new AuthenticateService(userRepositoryMock.Object);
            AuthenticateResult results = await service.AuthenticateUser(model);

            // Assert
            results.Identity.Should().NotBe(null);
            string nameClaim = results.Identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            nameClaim.Should().Be(model.Email);
        }

        private Mock<IUserRepository> SetupUserRepositoryMock(UserLoginResponse Response)
        {
            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(Response));
            return userRepositoryMock;
        }

    }
}

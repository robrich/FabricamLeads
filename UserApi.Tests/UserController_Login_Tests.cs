using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fabricam.UserApi;
using Fabricam.UserApi.Models;
using Fabricam.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace UserApi.Tests
{
    /// <summary>
    /// These are integration tests
    /// </summary>
    public class UserController_Login_Tests : IDisposable
    {
        private readonly TestServer server;
        private readonly HttpClient client;
        private const string LOGIN_URL = "/api/User/login";
        private const string VALID_EMAIL = "valid@user.com";
        private const string VALID_PASSWORD = "P@ssw0rd1!";

        public UserController_Login_Tests()
        {
            // Setup
            this.server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            this.client = this.server.CreateClient();
        }

        [Fact]
        public async Task ValidUser_Succeeds()
        {
            // Arrange
            UserLoginRequest req = new UserLoginRequest {
                Email = VALID_EMAIL,
                Password = VALID_PASSWORD
            };
            const bool expectedUserIsValid = true;

            // Act
            HttpResponseMessage res = await this.client.PostJsonAsync(LOGIN_URL, req);
            UserLoginResponse result = await res.ReadJsonResponseAsync<UserLoginResponse>();

            // Assert
            // Not matching the whole object since we don't want to validate message content, not i18n or l10n safe
            result.UserIsValid.Should().Be(expectedUserIsValid);
            result.UserId.Should().BeGreaterThan(0);
            result.Errors.Should().BeNullOrEmpty();
            ((int)(res.StatusCode)).Should().Be(200);
        }

        [Theory]
        [InlineData("notavalidemail")]
        [InlineData("")]
        [InlineData((string)null)]
        public async Task InvalidEmail_Fails(string Email)
        {
            // Arrange
            UserLoginRequest req = new UserLoginRequest {
                Email = Email,
                Password = VALID_PASSWORD
            };

            // Act & Assert
            await this.Run_Fails(req, nameof(req.Email));
        }

        [Theory]
        [InlineData("no")] // too short
        [InlineData("")]
        [InlineData((string)null)]
        public async Task InvalidPassword_Fails(string Password)
        {
            // Arrange
            UserLoginRequest req = new UserLoginRequest {
                Email = VALID_EMAIL,
                Password = Password
            };

            // Act & Assert
            await this.Run_Fails(req, nameof(req.Password));
        }

        [Fact]
        public async Task NullBody_Fails()
        {
            // Arrange
            UserLoginRequest req = null;
            const bool expectedUserIsValid = false;

            // Act
            HttpResponseMessage res = await this.client.PostJsonAsync(LOGIN_URL, req);
            UserLoginResponse result = await res.ReadJsonResponseAsync<UserLoginResponse>();

            // Assert
            // Not matching the whole object since we don't want to validate message content, not i18n or l10n safe
            result.UserIsValid.Should().Be(expectedUserIsValid);
            result.Errors.Should().HaveCount(1);
            result.Errors.FirstOrDefault().Field.Should().BeNullOrEmpty();
            ((int)(res.StatusCode)).Should().Be(422);
        }

        private async Task Run_Fails(UserLoginRequest req, string ErrorFieldName)
        {

            // Arrange
            const bool expectedUserIsValid = false;
            
            // Act
            HttpResponseMessage res = await this.client.PostJsonAsync(LOGIN_URL, req);
            UserLoginResponse result = await res.ReadJsonResponseAsync<UserLoginResponse>();

            // Assert
            // Not matching the whole object since we don't want to validate message content, not i18n or l10n safe
            result.UserIsValid.Should().Be(expectedUserIsValid);
            result.Errors.Count.Should().BeInRange(1, 2); // valid and maybe required
            result.Errors.ForEach(e => e.Field.Should().Be(ErrorFieldName));
            ((int)(res.StatusCode)).Should().Be(422);
        }

        public void Dispose()
        {
            this.server?.Host.StopAsync().Wait(); // FRAGILE: async method in a sync interface
        }

    }
}

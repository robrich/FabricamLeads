using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Fabricam.Web.Core.Models;
using Fabricam.Web.Core.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Fabricam.Web.Core.Services
{
    public interface IAuthenticateService
    {
        Task<AuthenticateResult> AuthenticateUser(AccountLoginModel Model);
    }

    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUserRepository userRepository;

        public AuthenticateService(IUserRepository UserRepository)
        {
            this.userRepository = UserRepository ?? throw new ArgumentNullException(nameof(UserRepository));
        }
        
        public async Task<AuthenticateResult> AuthenticateUser(AccountLoginModel Model)
        {
            // ASSUME: ModelState.IsValid validated previously

            AuthenticateResult results = new AuthenticateResult();

            var userResponse = await this.userRepository.LoginAsync(Model.Email, Model.Password);

            if (!userResponse.Success) {
                results.Errors = userResponse.Errors;
                return results;
            }

            if (!userResponse.UserIsValid) {
                results.Errors.Add(new ValidationError { Message = "User authentication failure" });
                return results;
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Model.Email),
                new Claim(ClaimTypes.Sid, userResponse.UserId.ToString())
            };

            results.Identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return results;
        }
    }
}

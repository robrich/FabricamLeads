using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Fabricam.Shared;
using Fabricam.Web.Core.Models;

namespace Fabricam.Web.Core.Repositories
{
    public interface IUserRepository
    {
        Task<UserCreateResponse> CreateUserAsync(UserCreateRequest Request);
        Task<UserLoginResponse> LoginAsync(string Email, string Password);
    }

    public class UserRepository : IUserRepository
    {
        private readonly string baseUrl;

        public UserRepository(AppSettings AppSettings)
        {
            if (AppSettings == null) {
                throw new ArgumentNullException(nameof(AppSettings));
            }

            this.baseUrl = AppSettings.UserApiUrl;
            if (!this.baseUrl.EndsWith("/")) {
                this.baseUrl += "/";
            }
            this.baseUrl += "api/User/";
        }

        public async Task<UserCreateResponse> CreateUserAsync(UserCreateRequest Request)
        {
            HttpClient client = new HttpClient(); // TODO: get this from DI if we need interesting logging or proxying
            HttpResponseMessage res = await client.PostJsonAsync(this.baseUrl + "create", Request);
            return (await res.ReadJsonResponseAsync<UserCreateResponse>()) ?? new UserCreateResponse {
                Errors = new List<ValidationError> {
                    new ValidationError {Message = "Error communicationg with user service"}
                }
            };
        }

        public async Task<UserLoginResponse> LoginAsync(string Email, string Password)
        {
            HttpClient client = new HttpClient(); // TODO: get this from DI if we need interesting logging or proxying
            HttpResponseMessage res = await client.PostJsonAsync(this.baseUrl + "login", new UserLoginRequest {
                Email = Email,
                Password = Password
            });
            return (await res.ReadJsonResponseAsync<UserLoginResponse>()) ?? new UserLoginResponse {
                Errors = new List<ValidationError> {
                    new ValidationError {Message = "Error communicationg with user service"}
                }
            };
        }
        
    }
}

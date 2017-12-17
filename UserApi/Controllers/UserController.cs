using System;
using Fabricam.UserApi.Filters;
using Fabricam.UserApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fabricam.UserApi.Controllers
{
    [ValidateModel]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        // CreateUser
        [HttpPost("create")]
        public UserCreateResponse Post([FromBody]UserCreateRequest Model)
        {
            // TODO: insert the user in the db and return the UserId
            int userId = this.GetFakeUserId();
            return new UserCreateResponse
            {
                UserId = userId
            };
        }

        // Login
        [HttpPost("login")]
        public UserLoginResponse Post([FromBody]UserLoginRequest Model)
        {
            // TODO: get the user from the db, validate user is active and not locked out, hash and compare the password, and return if the user is valid
            bool userIsValid = true;
            int userId = GetFakeUserId();
            return new UserLoginResponse
            {
                UserIsValid = userIsValid,
                UserId = userId
            };
        }
        
        // TODO: add other user maintenance methods:
        // - Change password
        // - Recover password (start and complete)
        // - Change user profile details: name, email, phone, etc

        private int GetFakeUserId() => new Random().Next(4999) + 1; // 1-5000

    }
}

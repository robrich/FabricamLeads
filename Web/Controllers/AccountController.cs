using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Fabricam.Web.Core.Models;
using Fabricam.Web.Core.Repositories;
using Fabricam.Web.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Fabricam.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IAuthenticateService authenticateService;

        public AccountController(IUserRepository UserRepository, IAuthenticateService AuthenticateService)
        {
            this.userRepository = UserRepository ?? throw new ArgumentNullException(nameof(UserRepository));
            this.authenticateService = AuthenticateService ?? throw new ArgumentNullException(nameof(AuthenticateService));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AccountCreateModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountCreateModel Model)
        {
            if (!this.ModelState.IsValid) {
                return View(Model ?? new AccountCreateModel()); // Fix your errors please
            }

            UserCreateResponse createResponse = await this.userRepository.CreateUserAsync(new UserCreateRequest {
                Email = Model.Email,
                Password = Model.Password
            });

            if (!createResponse.Success) {
                foreach (var error in createResponse.Errors) {
                    this.ModelState.AddModelError(error.Field, error.Message);
                }
                return View(Model);
            }

            return RedirectToAction("CreateSuccess");
        }

        public IActionResult CreateSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            // FRAGILE: because we're posting back to the same URL we came from, we need not copy `string ReturnUrlParameter` to the model
            return View(new AccountLoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginModel Model)
        {
            if (!this.ModelState.IsValid) {
                return View(Model ?? new AccountLoginModel()); // Fix your errors please
            }

            var authResponse = await this.authenticateService.AuthenticateUser(Model);

            if (!authResponse.Success) {
                foreach (var error in authResponse.Errors) {
                    this.ModelState.AddModelError(error.Field, error.Message);
                }
                return View(Model);
            }

            if (authResponse.Identity == null) {
                this.ModelState.AddModelError("", "User authentication failure");
                return View(Model);
            }
            
            // Only accept redirect url if it's local, prevent reflection attack:
            string url = "/";
            if (!string.IsNullOrEmpty(Model.ReturnUrl) && Url.IsLocalUrl(Model.ReturnUrl)) { 
                url = Model.ReturnUrl;
            }

            await this.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(authResponse.Identity),
                new AuthenticationProperties {IsPersistent = Model.RememberMe}
            );

            return this.LocalRedirect(url);
        }
        
        [HttpGet] // Technically an XSS hole, but a much better user experience than [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LogoutSuccess");
        }

        public IActionResult LogoutSuccess()
        {
            return View();
        }

    }
}

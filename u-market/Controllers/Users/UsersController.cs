using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using u_market.DAL;
using u_market.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using u_market.Controllers.Users;

namespace u_market.Controllers
{
    [Authorize(Roles = "Admin,Client")]
    public class UsersController : Controller
    {
        private readonly MarketContext Ctx;
        private UsersLogic Logic;

        public UsersController(MarketContext context)
        {
            Ctx = context;
            Logic = new UsersLogic(context);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            // authenticated users cannot login
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string Username, string Password)
        {
            var user = Logic.FindUser(Username, Password);

            if (user != null)
            {
                await RegisterCookieForUser(user);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Username or Password are incorrect";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        private async Task RegisterCookieForUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("Username", user.Username),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        [AllowAnonymous]
        public IActionResult OnPostUserChecking(string Username)
        {
            return Json(Logic.IsUsernameAvailable(Username));
        }

        [AllowAnonymous]
        public IActionResult RegisterUser(User newUser)
        {
            if (Logic.IsUsernameAvailable(newUser.Username))
            {
                Ctx.Add(newUser);
                newUser.UserRole = Role.Client;
                Ctx.SaveChanges();

                return RedirectToAction(nameof(Login));
            }

            ViewBag.Error = "User name is an available";
            return RedirectToAction(nameof(Register));
        }


        [AllowAnonymous]
        public IActionResult Register()
        {
            // authenticated users cannot register
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
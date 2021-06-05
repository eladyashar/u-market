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

namespace u_market.Controllers
{
    [Authorize(Roles = "Admin,Client")]
    public class UsersController : Controller
    {
        private readonly MarketContext Ctx;
        private readonly UsersManagementLogic UsersManagementLogic;

        public UsersController(MarketContext context)
        {
            Ctx = context;
            UsersManagementLogic = new UsersManagementLogic(Ctx);
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
            var user = Ctx.Users.SingleOrDefault(user => user.Username == Username && user.Password == Password);

            if (user != null)
            {
                await RegisterCookieForUser(user);
                return RedirectToAction("Index", "Home");
            }

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
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private bool IsUsernameAvailable(string username)
        {
            bool isUsernameAvailable = Ctx.Users.SingleOrDefault(user => user.Username == username) == null;
            return isUsernameAvailable;
        }

        [AllowAnonymous]
        public IActionResult OnPostUserChecking(string Username)
        {
            return Json(IsUsernameAvailable(Username));
        }

        [AllowAnonymous]
        public IActionResult RegisterUser(User newUser)
        {
            if (IsUsernameAvailable(newUser.Username))
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
                return RedirectToAction("Home", "Genres");
            }

            return View();
        }

        public IActionResult UsersManagement()
        {
            List<User> allUsers = UsersManagementLogic.GetAll().ToList();
            return View();
        }
    }
}
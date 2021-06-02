using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using u_market.DAL;
using u_market.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace u_market.Controllers
{
    public class UsersController : Controller
    {
        private readonly MarketContext Ctx;

        public UsersController(MarketContext context)
        {
            Ctx = context;
        }

        public IActionResult Login()
        {
            // authenticated users cannot login
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Home", "Index");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Username, string Password)
        {
            var users = Ctx.Users.Where(user => user.Username == Username && user.Password == Password).ToList();
            ViewData["FailedLogin"] = false;

            if (users != null && users.Count() > 0)
            {
                await RegisterCookieForUser(users.First());
                return RedirectToAction("Home", "Index");
            }

            ViewData["FailedLogin"] = true;

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
                //new Claim("FirstName", user.FirstName),
                //new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

        }

        [HttpPost]
        public ActionResult IsUsernameAvailable(string username)
        {
            bool isUsernameAvailable = Ctx.Users.SingleOrDefault(user => user.Username == username) != null;

            return Json(new { isUsernameAvailable });
        }

        public IActionResult Register()
        {
            // authenticated users cannot register
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Home", "Genres");
            }

            return View();
        }
    }
}
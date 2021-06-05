using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using u_market.DAL;
using u_market.Models;

namespace u_market.Controllers.Users
{
    [Authorize(Roles = "Admin")]
    public class UsersManagementController : Controller
    {
        private UsersManagementLogic Logic { get; }

        public UsersManagementController(MarketContext ctx)
        {
            Logic = new UsersManagementLogic(ctx);
        }

        public IActionResult Index()
        {
            ViewBag.Users = Logic.GetAll();
            return View();

        }

        [HttpDelete]
        public void Delete([FromBody] string username)
        {
            var user = Logic.FindUser(username);
            Logic.RemoveUser(user);
            ViewBag.Users = Logic.GetAll();
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using u_market.DAL;

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
    }
}
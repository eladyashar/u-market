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
            return View();
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery(Name = "query")]string? query)
        {
            return Ok(Logic.GetAll(query));
        }

        [HttpPut]
        public IActionResult Update([FromBody] User user)
        {
            Logic.Update(user);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] string username)
        {
            var user = Logic.FindUser(username);
            Logic.RemoveUser(user);
            return Ok();
        }
    }
}
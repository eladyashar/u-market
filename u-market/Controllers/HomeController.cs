using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using u_market.Models;
using u_market.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace u_market.Controllers
{
    [Authorize(Roles = "Admin,Client")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MarketContext Ctx;
        public HomeController(MarketContext Ctx)
        {
            this.Ctx = Ctx;
        }

        public IActionResult Index()
        {
            ViewBag.Products = GetAll();
            return View();
        }

        private List<Product> GetAll()
        {
            return Ctx.Products.Include(p => p.Store).ToList();
        }

        public ActionResult Privacy(string name, int numTimes = 1)
        {
            ViewBag.Message = "Hello World!" + name;
            //ViewBag.ProductNames = logic.GetAll().Select(x => x.Name);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

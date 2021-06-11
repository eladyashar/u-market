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
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Query;

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

        public IActionResult Index([FromQuery(Name = "store")] int store, [FromQuery(Name = "price")] double price)
        {
            ViewBag.Stores = GetAllStores();
            ViewBag.Prices = GetPrices();
            ViewBag.Products = GetAll(store, price);
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            ViewBag.Username = claims.ToArray()[0].ToString().Split(' ')[1];
            return View();
        }

        private IList<Product> GetAll(int storeId, double price)
        {
            IQueryable<Product> products = this.Ctx.Products;

            if (storeId != 0)
            {
                products = products.Where(p => p.StoreId == storeId);
            }

            if (price != 0)
            {
                products = products.Where(p => p.Price == price);
            }

            return products.Include(p => p.Store).ToList();
        }

        private IList<double> GetPrices()
        {
            return this.Ctx.Products.GroupBy(p => p.Price).Select(p => p.Key).ToList();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // TODO: move to store logic
        private IList<Store> GetAllStores()
        {
            return this.Ctx.Stores.ToList();
        }
    }
}

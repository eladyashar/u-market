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

        public IActionResult Index([FromQuery(Name = "query")] string? query, [FromQuery(Name = "store")] int? store, [FromQuery(Name = "price")] double? price, [FromQuery(Name = "tag")] int? tag)
        {
            ViewBag.Products = GetAll(query, store, price, tag);

            ViewBag.Stores = GetAllStores();
            ViewBag.Prices = GetPrices();
            ViewBag.Tags = GetTags();
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            ViewBag.Username = claims.ToArray()[0].ToString().Split(' ')[1];
            return View();
        }

        private IList<Product> GetAll(string? query, int? storeId, double? price, int? tagId)
        {
            IQueryable<Product> products = this.Ctx.Products.Include(p => p.Store);
            
            if (query != null)
            {
                products = products.Where(p => p.Name.Contains(query) || 
                                            p.Tags.Any(t => t.Name.Contains(query)) || 
                                            p.Store.Name.Contains(query) ||
                                            p.Description.Contains(query) ||
                                            p.Price.ToString().Contains(query));
            }

            if (storeId != null)
            {
                products = products.Where(p => p.StoreId == storeId);
            }

            if (price != null)
            {
                products = products.Where(p => p.Price == price);
            }

            if (tagId != null)
            {
                products = products.Include(p => p.Tags).Where(p => p.Tags.Where(t  => t.Id == tagId).Count() >= 1);
            }

            return products.ToList();
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

        private IList<Tag> GetTags()
        {
            return this.Ctx.Tags.ToList();
        }


        [Route("/Views/Error/404.cshtml")]
        public IActionResult HandleError(int code)
        {
            return View("~/Views/Error/404.cshtml");
        }
    }
}

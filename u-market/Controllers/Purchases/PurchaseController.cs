using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u_market.DAL;
using Microsoft.EntityFrameworkCore;
using u_market.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace u_market.Controllers.Purchases
{
    public class PurchaseController : Controller
    {
        private readonly MarketContext Ctx;
        public PurchaseController(MarketContext Ctx)
        {
            this.Ctx = Ctx;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.Purchases = getAll();
            return View();
        }
        public IList<Purchase> getAll()
        {
            return Ctx.Purchases.Include(c=>c.Product).Include(p=> p.User).ToList();
        }
    }
}
 
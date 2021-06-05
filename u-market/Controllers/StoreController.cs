using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using u_market.DAL;
using u_market.Models;
using Microsoft.EntityFrameworkCore;
namespace u_market.Controllers
{
    public class StoreController : Controller
    {
        private readonly MarketContext Ctx;

        public StoreController(MarketContext Ctx)
        {
            this.Ctx = Ctx;
        }

        public ActionResult Index()
        {
            Store myStore = GetAll().Where(s => s.OwnerId == "ramig")
                    .FirstOrDefault<Store>();
            //string jsonString = JsonSerializer.Serialize(myStore);
            ViewBag.StoreDetails = myStore;
            return View();
        }

        private IList<Store> GetAll()
        {
            return Ctx.Stores.Include(c => c.Products)
                .Include(c=> c.Owner).ToList();
        }
    }
}

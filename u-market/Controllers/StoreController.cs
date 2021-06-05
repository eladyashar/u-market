using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using u_market.DAL;
using u_market.Models;
using Microsoft.EntityFrameworkCore;
namespace u_market.Controllers
{
    public class StoreController : Controller
    {
        private readonly MarketContext Ctx;
        private readonly ProductLogic productLogic;

        public StoreController(MarketContext Ctx)
        {
            this.productLogic = new ProductLogic(Ctx);
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

        public ActionResult AddProduct([Bind("Id,Name,StoreId,Price,ImageUrl,Description")] Product product)
        {
            productLogic.AddProduct(product);
            return View("~/Views/Store/index.cshtml");
        }

        private IList<Store> GetAll()
        {
            var stores = Ctx.Stores.Include(c => c.Products)
                .Include(c => c.Owner);
            return stores.ToList();
        }
    }
}

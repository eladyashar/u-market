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
            ViewBag.StoreDetails = myStore;
            return View();
        }

        public IActionResult UpdateStore([Bind("Id,Name,Lat,Lang,OwnerId")] Store store)
        {
            try
            {
                Ctx.Stores.Update(store);
                Ctx.SaveChanges();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        public IActionResult AddProduct([Bind("Name,StoreId,Price,ImageUrl,Description")] Product product)
        {
            try
            {
                productLogic.AddProduct(product);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        public IActionResult UpdateProduct([Bind("Id,Name,StoreId,Price,ImageUrl,Description")] Product product)
        {
            try
            {
                productLogic.UpdateProduct(product);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        public IActionResult RemoveProduct([Bind("productId")] int productId)
        {
            try
            {
                productLogic.RemoveProduct(productId);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        private IList<Store> GetAll()
        {
            var stores = Ctx.Stores.Include(c => c.Products)
                .Include(c => c.Owner);
            return stores.ToList();
        }
    }
}

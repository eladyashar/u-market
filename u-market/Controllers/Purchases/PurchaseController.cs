using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u_market.DAL;
using Microsoft.EntityFrameworkCore;
using u_market.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace u_market.Controllers.Purchases
{
    public class PurchaseController : Controller
    {
        private PurchaseLogic Logic { get; }

        public PurchaseController(MarketContext Ctx)
        {
            Logic = new PurchaseLogic(Ctx);
        }

        public IActionResult Index([FromQuery(Name = "productName")] int? productId, [FromQuery(Name = "tag")] int? tag, [FromQuery(Name = "date")] string? date)
        {
            ViewBag.Purchases = GetAll(productId, tag, date);
            ViewBag.PurchasesDates = GetPurchasesDates();
            ViewBag.Stores = GetAllStores();
            ViewBag.Products = GetProducts();
            ViewBag.Tags = GetTags();
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public IActionResult GetAll(int? productId, int? tag, string? date)
        {
            return Ok(Logic.GetAll(productId, tag, date));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public IActionResult GetAllStores()
        {
            return Ok(Logic.GetAllStores());
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public IActionResult GetProducts()
        {
            return Ok(Logic.GetProducts());
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public IActionResult GetTags()
        {
            return Ok(Logic.GetTags());
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public IActionResult GetPurchasesDates()
        {
            return Ok(Logic.GetPurchasesDates());
        }


    }
}
 
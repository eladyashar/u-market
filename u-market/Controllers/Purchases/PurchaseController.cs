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
    [Authorize(Roles = "Admin")]
    public class PurchaseController : Controller
    {
        private PurchaseLogic Logic { get; }

        public PurchaseController(MarketContext Ctx)
        {
            Logic = new PurchaseLogic(Ctx);
        }

        public IActionResult Index([FromQuery(Name = "query")] string? query, [FromQuery(Name = "productName")] int? productId, [FromQuery(Name = "tag")] int? tag,
                                   [FromQuery(Name = "date")] string? date, [FromQuery(Name = "storeId")] int? storeId)
        {
            ViewBag.Purchases = GetAll(query, productId, tag, date, storeId);
            ViewBag.PurchasesDates = GetPurchasesDates();
            ViewBag.Stores = GetAllStores();
            ViewBag.Products = GetProducts();
            ViewBag.Tags = GetTags();
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public IActionResult GetAll(string? query, int? productId, int? tag, string? date, int? storeId)
        {
            return Ok(Logic.GetAll(query, productId, tag, date, storeId));
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
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u_market.DAL;
using Microsoft.EntityFrameworkCore;
using u_market.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace u_market.Controllers.Purchases
{
    public class PurchaseController : Controller
    {
        private PurchaseLogic Logic { get; }

        public PurchaseController(MarketContext Ctx)
        {
            Logic = new PurchaseLogic(Ctx);
        }

        public IActionResult Index()
        {
            @ViewBag.Data = GetAll("");
            @ViewBag.Purchases = @ViewBag.Data.Value;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public IActionResult GetAll(string? filter)
        {
            return Ok(Logic.GetAll(filter));
        }

    }
}
 
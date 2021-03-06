using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using u_market.DAL;
using u_market.Models;

namespace u_market.Controllers
{
    [Authorize(Roles = "Admin,Client")]
    public class ProductController : Controller
    {
        private readonly MarketContext Ctx;

        public ProductController(MarketContext Ctx)
        {
            this.Ctx = Ctx;
        }

        public IActionResult AddPurchase([Bind("Username,ProductId,PurchaseDate")] Purchase purchase)
        {
            try
            {
                Ctx.Purchases.Add(purchase);
                Ctx.SaveChanges();
            }
            catch
            {
                return BadRequest(new { Message = "Something went wrong" });
            }

            return Ok();
        }
    }
}

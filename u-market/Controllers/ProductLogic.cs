using u_market.DAL;
using u_market.Models;

namespace u_market.Controllers
{
    public class ProductLogic
    {
        protected MarketContext Ctx { get; set; }

        public ProductLogic(MarketContext ctx)
        {
            Ctx = ctx;
        }

        public void AddProduct(Product product)
        {
            Ctx.Products.Add(product);
            Ctx.SaveChanges();
        }
    }
}
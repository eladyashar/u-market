using System.Collections.Generic;
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

        public void AddProduct(Product product, List<Tag> tags)
        {
            Ctx.Products.Add(product);

            foreach (Tag tag in tags)
            {
                Ctx.Tags.Add(tag);
                Ctx.Attach(tag);
                product.Tags.Add(tag);
            }

            Ctx.SaveChanges();
        }

        public void RemoveProduct(int productId)
        {
            Ctx.Products.Remove(FindProduct(productId));
            Ctx.SaveChanges();
        }

        public void UpdateProduct(Product product, List<Tag> tags)
        {

            Ctx.Products.Update(product);
            Ctx.SaveChanges();
        }

        public Product FindProduct(int productId)
        {
            return Ctx.Products.Find(productId);
        }
    }
}
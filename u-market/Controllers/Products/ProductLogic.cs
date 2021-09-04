using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using u_market.Controllers.Tags;
using u_market.DAL;
using u_market.Models;
using System.Linq;

namespace u_market.Controllers
{
    public class ProductLogic
    {
        protected MarketContext Ctx { get; set; }
        private TagLogic TagLogic { get; set; }

        public ProductLogic(MarketContext ctx)
        {
            Ctx = ctx;
            TagLogic = new TagLogic(ctx);
        }

        public Product FindProduct(int productId)
        {
            return Ctx.Products.Find(productId);
        }

        public void AddProduct(Product product, List<Tag> tags)
        {
            Ctx.Products.Add(product);

            SetTagsInProduct(product, tags);

            Ctx.SaveChanges();
        }

        private void SetTagsInProduct(Product product, List<Tag> tags)
        {
            foreach (Tag tag in tags)
            {
                Ctx.Tags.Add(tag);
                Ctx.Tags.Attach(tag);
                product.Tags.Add(tag);
            }
        }

        public void UpdateProduct(Product product, List<Tag> tags)
        {
            Ctx.Products.Update(product);
            Ctx.SaveChanges();

            UnsetTagsInProduct(product.Id);
            
            Ctx.Products.Add(product);
            Ctx.Products.Attach(product);

            SetTagsInProduct(product, tags);
            
            Ctx.SaveChanges();
        }

        private void UnsetTagsInProduct(int productId)
        {
            var product = Ctx.Products.Include(p => p.Tags).SingleOrDefault(p => p.Id == productId);

            var copiedTags = new HashSet<Tag>(product.Tags);

            foreach (Tag tag in copiedTags)
            {
                product.Tags.Remove(tag);
            }

            Ctx.SaveChanges();

            foreach (Tag tag in copiedTags)
            {
                Ctx.Entry(tag).State = EntityState.Detached;
            }

            Ctx.Entry(product).State = EntityState.Detached;
        }

        public void RemoveProduct(int productId)
        {
            Ctx.Products.Remove(FindProduct(productId));
            Ctx.SaveChanges();
        }
    }
}
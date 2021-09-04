using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using u_market.Controllers.Tags;
using u_market.DAL;
using u_market.Models;
using System.Linq;
using System;
using u_market.Exceptions;

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
            EnsureProduct(product, tags);

            Ctx.Products.Add(product);

            SetTagsInProduct(product, tags);

            Ctx.SaveChanges();
        }

        private void EnsureProduct(Product product, List<Tag> tags)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ModelValidationException("Product name cannot be empty");
            }

            if (product.Price <= 0)
            {
                throw new ModelValidationException("Product price must be positive");
            }

            if (string.IsNullOrWhiteSpace(product.Description))
            {
                throw new ModelValidationException("Product description cannot be empty");
            }

            if (product.StoreId == 0)
            {
                throw new ModelValidationException("Product must be linked to store");
            }

            tags.ForEach(tag =>
            {
                if (string.IsNullOrWhiteSpace(tag.Name))
                {
                    throw new ModelValidationException("Product's tags must have a name");
                }
            });
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
            EnsureProduct(product, tags);

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
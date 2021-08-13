using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u_market.DAL;
using u_market.Models;

namespace u_market.Controllers.Purchases
{
    public class PurchaseLogic
    {
        private MarketContext Ctx { get; }
        public PurchaseLogic(MarketContext Ctx)
        {
            this.Ctx = Ctx;
        }

        public IList<Purchase> GetAll(int? productId, int? tag, string? date)
        {
            IQueryable<Purchase> purchases = this.Ctx.Purchases.Include(c => c.Product);
            //IQueryable<Purchase> purchases = this.Ctx.Purchases.Include(c => c.Product).Include(p => p.User);

            if (productId != null)
            {
                purchases = purchases.Where(p => p.Product.Id == productId);
            }

            if (tag != null)
            {
                purchases = purchases.Include(p => p.Product.Tags).Where(p => p.Product.Tags.Where(t => t.Id == tag).Count() >= 1);
            }

            if (date != null)
            {
                purchases = purchases.Where(p => p.PurchaseDate.ToString().Contains(date));
            }

            //return purchases.Include(c => c.Product).Include(p => p.User).OrderBy(p => p.ProductId).ToList();
            return purchases.ToList();

            //var query = this.Ctx.Purchases.AsQueryable();

            //if (filter != null)
            //{
            //    query = query.Where(t => t.Product.Name.Contains(filter));
            //}

            //return query.Include(c => c.Product).Include(p => p.User).OrderBy(p => p.ProductId).ToList();
        }
        public IList<Store> GetAllStores()
        {
            return this.Ctx.Stores.ToList();
        }

        public IList<Product> GetProducts()
        {
            return this.Ctx.Products.ToList();
        }

        public IList<Tag> GetTags()
        {
            return this.Ctx.Tags.ToList();
        }

        public IList<String> GetPurchasesDates()
        {
            return this.Ctx.Purchases.Select(p => p.PurchaseDate.Date.ToString()).ToList();
        }

        


    }
}

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

        public IList<Purchase> GetAll(string? query, int? productId, int? tag, string? date, int? storeId)
        {
            IQueryable<Purchase> purchases = this.Ctx.Purchases.Include(c => c.Product);

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

            if (storeId != null)
            {
                purchases = purchases.Where(p => p.Product.Store.Id == storeId);
            }

            if (query != null)
            {
                purchases = purchases.Where(p => p.Product.Name.Contains(query) || p.Product.Store.Name.Contains(query) || p.Username.Contains(query) || p.PurchaseDate.ToString().Contains(query));
            }

            return purchases.ToList();
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
            return this.Ctx.Purchases.GroupBy(p => p.PurchaseDate.Date).Select(p => p.Key.Date.ToString()).ToList();
        }
    }
}

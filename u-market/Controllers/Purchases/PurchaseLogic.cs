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

        public IList<Purchase> GetAll(string? filter)
        {
            var query = this.Ctx.Purchases.AsQueryable();

            if (filter != null)
            {
                //query = query.Where(p => p.Product(filter));
            }

            return query.Include(c => c.Product).Include(p => p.User).OrderBy(p => p.ProductId).ToList();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u_market.DAL;
using u_market.Models;

namespace u_market.Controllers.Stores
{
    public class StoreLogic
    {
        protected MarketContext Ctx { get; set; }

        public StoreLogic(MarketContext ctx)
        {
            Ctx = ctx;
        }

        public IEnumerable<Store> GetAll()
        {
            return Ctx.Stores.Include(store => store.Owner).Include(store => store.Products).ToList();
        }

        public void Insert(Store store)
        {
            Ctx.Stores.Add(store);
            Ctx.SaveChanges();
        }

        public void Update(Store store)
        {
            Ctx.Stores.Update(store);
            Ctx.SaveChanges();
        }
    }
}
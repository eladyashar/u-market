using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using u_market.DAL;
using u_market.Exceptions;
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

        public IEnumerable<Store> GetAll(string? query)
        {
            IQueryable<Store> stores = Ctx.Stores.Include(store => store.Owner).Include(store => store.Products).ThenInclude(product => product.Tags);
            
            if (query != null)
            {
                stores = stores.Where(store => store.Name.Contains(query) || store.Owner.FirstName.Contains(query) || store.Owner.LastName.Contains(query));
            }

            return stores.ToList();
        }

        public Store FindStore(int storeId)
        {
            return Ctx.Stores.Include(s => s.Products).SingleOrDefault(store => store.Id == storeId);
        }

        public Store FindMyStore(String userId) 
        {
            return Ctx.Stores.Where(s => s.OwnerId.Equals(userId)).Include(store => store.Owner).Include(store => store.Products).FirstOrDefault();
        }

        public void Insert(Store store, ClaimsPrincipal user)
        {
            store.OwnerId = user.Claims.Single(c => c.Type.Equals("Username")).Value;

            EnsureStore(store);

            Ctx.Stores.Add(store);
            Ctx.SaveChanges();
        }

        private void EnsureStore(Store store)
        {
            if (string.IsNullOrWhiteSpace(store.Name))
            {
                throw new ModelValidationException("Store name cannot be empty");
            }
            else if(store.Name.Length > 20)
            {
                throw new ModelValidationException("Store name cannot be be more than 20 sign");
            }

            if (store.Lat == 0 && store.Lang == 0)
            {
                throw new ModelValidationException("Store address cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(store.OwnerId))
            {
                throw new ModelValidationException("Store must have an owner");
            }
        }

        public void Update(Store store)
        {
            EnsureStore(store);

            Ctx.Stores.Update(store);
            Ctx.SaveChanges();
        }

        public void Delete(int storeId)
        {
            Ctx.Stores.Remove(FindStore(storeId));
            Ctx.SaveChanges();
        }
    }
}
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using u_market.DAL;
using u_market.Models;

namespace u_market.Controllers.Statistics
{
    public class StatisticsLogic
    {
        protected MarketContext Ctx { get; set; }

        public StatisticsLogic(MarketContext ctx)
        {
            Ctx = ctx;
        }

        public IDictionary<string, int> GetAll()
        {
            return Ctx.Purchases.Join(Ctx.Products,
                                        purchase => purchase.ProductId,
                                        product => product.Id,
                                            (purchase, product) => new
                                                              {
                                                                  productName = product.Name
                                                                })
                                                            .AsEnumerable().GroupBy(p => p.productName).ToDictionary(x => x.Key, x => x.Count());
        }

        //public User FindUser(string username)
        //{
        //    return Ctx.Users.SingleOrDefault(user => user.Username == username);
        //}
    }
}
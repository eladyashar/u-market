using Microsoft.EntityFrameworkCore;
using System.Linq;
using u_market.Controllers.Users;
using u_market.DAL;
using u_market.Models;

namespace u_market.Controllers
{
    public class UsersManagementLogic : UsersLogic
    {
        public UsersManagementLogic(MarketContext ctx) : base(ctx)
        {

        }

        public void RemoveUser(User user)
        {
            var store = Ctx.Stores.Include(s => s.Products).SingleOrDefault(s => s.Owner == user);
            Ctx.Users.Remove(user);
            Ctx.SaveChanges();
        }

        public void Update(User user)
        {
            Ctx.Users.Update(user);
            Ctx.SaveChanges();
        }
    }
}
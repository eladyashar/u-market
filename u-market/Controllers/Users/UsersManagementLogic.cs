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
            Ctx.Users.Remove(user);
        }

        public void Update(User user)
        {
            Ctx.Users.Update(user);
        }
    }
}
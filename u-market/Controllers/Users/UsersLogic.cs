using System.Collections.Generic;
using System.Linq;
using u_market.DAL;
using u_market.Models;

namespace u_market.Controllers.Users
{
    public class UsersLogic
    {
        protected MarketContext Ctx { get; set; }

        public UsersLogic(MarketContext ctx)
        {
            Ctx = ctx;
        }

        public IEnumerable<User> GetAll()
        {
            return Ctx.Users.ToList();
        }

        public User FindUser(string username, string password)
        {
            return Ctx.Users.SingleOrDefault(user => user.Username == username && user.Password == password);
        }

        public bool IsUsernameAvailable(string username)
        {
            bool isUsernameAvailable = Ctx.Users.SingleOrDefault(user => user.Username == username) == null;
            return isUsernameAvailable;
        }

        public void AddUser(User user)
        {
            Ctx.Users.Add(user);
        }
    }
}
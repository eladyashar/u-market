using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public IEnumerable<User> GetAll(string? query)
        {
            IQueryable<User> users = Ctx.Users;

            if (query != null)
            {
                users = users.Where(u => u.FirstName.Contains(query) || u.LastName.Contains(query));
            }

            return users.ToList();
        }

        public User FindUser(string username)
        {
            return Ctx.Users.SingleOrDefault(user => user.Username == username);
        }

        public User FindUser(string username, string password)
        {
            return Ctx.Users.SingleOrDefault(user => user.Username == username && user.Password == password);
        }

        public bool IsUsernameAvailable(string username)
        {
            return FindUser(username) == null;
        }

        public void AddUser(User user)
        {
            Ctx.Users.Add(user);
            Ctx.SaveChanges();
        }
    }
}
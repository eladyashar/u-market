using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u_market.Models;

namespace u_market.DAL
{
    public static class DatabaseSeed
    {
        public static void Seed(MarketContext context)
        {
            if (!context.Users.Any())
            {
                initUsers(context);
            }
            if (!context.Stores.Any())
            {
                initStores(context);
            }
            if (!context.Tags.Any())
            {
                initTags(context);
            }
            if (!context.Products.Any())
            {
                initProducts(context);
            }
        }

        private static void initUsers(MarketContext context)
        {
            // Clients
            for (int i = 0; i < 5; i++)
            {
                User reader = new User()
                {
                    Username = "client" + i.ToString(),
                    Password = i.ToString(),
                    UserRole = Role.Client,
                    FirstName = "my-firstname-is-" + i,
                    LastName = "my-lastname-is-" + i
                };

                context.Add(reader);
            }

            // Admin
            User admin = new User()
            {
                Username = "ramig",
                Password = "ramig2",
                UserRole = Role.Admin,
                FirstName = "Rami",
                LastName = "Goldvarg"
            };

            context.Add(admin);

            context.SaveChanges();
        }
        private static void initStores(MarketContext context)
        {
            Store store = new Store()
            {
                Name = "Car Shop",
                Lat = 32.08544449947704,
                Lang = 34.81442400085823,
                OwnerId = "ramig"
            };

            context.Add(store);
            context.SaveChanges();
        }
        private static void initTags(MarketContext context)
        {
            for (int i = 0; i < 5; i++)
            {
                context.Add(new Tag()
                {
                    Name = "tag" + i.ToString()
                });
            }

            context.SaveChanges();
        }
        private static void initProducts(MarketContext context)
        {

            for (int i = 0; i < 5; i++)
            {
                context.Add(new Product()
                {
                    Name = "product" + i.ToString(),
                    Description = "product" + i.ToString(),
                    ImageUrl = null,
                    Price = i,
                    StoreId = 1,
                    //Tags = { new Tag() { Id = i }, new Tag() { Id = (i + 1) % 5 } }
                });
            }

            context.SaveChanges();
        }
    }
}

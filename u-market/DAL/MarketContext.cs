using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u_market.Models;

namespace u_market.DAL
{
    public class MarketContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<User> Users { get; set; }
        public MarketContext(DbContextOptions<MarketContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("products").Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Store>().ToTable("stores");
            modelBuilder.Entity<User>().ToTable("users");
        }
    }
}
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
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public MarketContext(DbContextOptions<MarketContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("products").Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().HasOne(p => p.Store).WithMany(s => s.Products).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Store>().ToTable("stores").Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Store>().HasOne(s => s.Owner).WithOne().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().ToTable("users");

            modelBuilder.Entity<Purchase>().ToTable("purchases").HasKey(c => new { c.Username, c.ProductId, c.PurchaseDate });
            modelBuilder.Entity<Purchase>().ToTable("purchases").HasOne(p => p.Product).WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Purchase>().ToTable("purchases").HasOne(p => p.User).WithMany().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tag>().ToTable("tags").Property(t => t.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Tag>().ToTable("tags").HasMany<Product>(t => t.Products).WithMany(p => p.Tags)
                .UsingEntity<Dictionary<string, object>>(
                "product_tags",
                pt => pt.HasOne<Product>().WithMany().HasForeignKey("product_id").OnDelete(DeleteBehavior.Cascade),
                pt => pt.HasOne<Tag>().WithMany().HasForeignKey("tag_id").OnDelete(DeleteBehavior.Cascade));
        }
    }
}   
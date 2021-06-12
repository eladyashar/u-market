﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using u_market.DAL;

namespace u_market.Migrations
{
    [DbContext(typeof(MarketContext))]
    partial class MarketContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("product_tag", b =>
                {
                    b.Property<int>("product_id")
                        .HasColumnType("integer");

                    b.Property<int>("tag_id")
                        .HasColumnType("integer");

                    b.HasKey("product_id", "tag_id");

                    b.HasIndex("tag_id");

                    b.ToTable("product_tag");
                });

            modelBuilder.Entity("u_market.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("price");

                    b.Property<int>("StoreId")
                        .HasColumnType("integer")
                        .HasColumnName("store_id");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.ToTable("products");
                });

            modelBuilder.Entity("u_market.Models.Purchase", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("character varying(20)")
                        .HasColumnName("username");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("time_stamp");

                    b.HasKey("Username", "ProductId", "PurchaseDate");

                    b.HasIndex("ProductId");

                    b.ToTable("purchases");
                });

            modelBuilder.Entity("u_market.Models.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Lang")
                        .HasColumnType("double precision")
                        .HasColumnName("lang");

                    b.Property<double>("Lat")
                        .HasColumnType("double precision")
                        .HasColumnName("lat");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("stores");
                });

            modelBuilder.Entity("u_market.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("tag_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("tags");
                });

            modelBuilder.Entity("u_market.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("username");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("password");

                    b.Property<int>("UserRole")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.HasKey("Username");

                    b.ToTable("users");
                });

            modelBuilder.Entity("product_tag", b =>
                {
                    b.HasOne("u_market.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("product_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("u_market.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("tag_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("u_market.Models.Product", b =>
                {
                    b.HasOne("u_market.Models.Store", "Store")
                        .WithMany("Products")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("u_market.Models.Purchase", b =>
                {
                    b.HasOne("u_market.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("u_market.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("u_market.Models.Store", b =>
                {
                    b.HasOne("u_market.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("u_market.Models.Store", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}

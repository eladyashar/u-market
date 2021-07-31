﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using u_market.DAL;

namespace u_market.Migrations
{
    [DbContext(typeof(MarketContext))]
    [Migration("20210605150142_purchase")]
    partial class purchase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:Identity", "1, 1");

            modelBuilder.Entity("u_market.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:Identity", "1, 1");

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

                    b.Property<int?>("store_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("store_id");

                    b.ToTable("products");
                });

            modelBuilder.Entity("u_market.Models.Purchase", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("character varying(20)");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("time_stamp");

                    b.HasKey("Username", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("purchases");
                });

            modelBuilder.Entity("u_market.Models.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:Identity", "1, 1");

                    b.Property<string>("Address")
                        .HasColumnType("text")
                        .HasColumnName("address");

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

            modelBuilder.Entity("u_market.Models.Product", b =>
                {
                    b.HasOne("u_market.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("store_id");

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
#pragma warning restore 612, 618
        }
    }
}

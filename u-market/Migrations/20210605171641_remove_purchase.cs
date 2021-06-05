using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace u_market.Migrations
{
    public partial class remove_purchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "purchases");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "purchases",
                columns: table => new
                {
                    username = table.Column<string>(type: "character varying(20)", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    time_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchases", x => new { x.username, x.product_id, x.time_stamp });
                    table.ForeignKey(
                        name: "FK_purchases_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_purchases_users_username",
                        column: x => x.username,
                        principalTable: "users",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_purchases_product_id",
                table: "purchases",
                column: "product_id");
        }
    }
}

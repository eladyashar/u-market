using Microsoft.EntityFrameworkCore.Migrations;

namespace u_market.Migrations
{
    public partial class store_owner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "stores",
                type: "character varying(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_stores_OwnerId",
                table: "stores",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_stores_users_OwnerId",
                table: "stores",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "username",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stores_users_OwnerId",
                table: "stores");

            migrationBuilder.DropIndex(
                name: "IX_stores_OwnerId",
                table: "stores");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "stores");
        }
    }
}

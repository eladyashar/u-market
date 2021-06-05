using Microsoft.EntityFrameworkCore.Migrations;

namespace u_market.Migrations
{
    public partial class store_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "stores");

            migrationBuilder.AddColumn<double>(
                name: "lang",
                table: "stores",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "lat",
                table: "stores",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lang",
                table: "stores");

            migrationBuilder.DropColumn(
                name: "lat",
                table: "stores");

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "stores",
                type: "text",
                nullable: true);
        }
    }
}

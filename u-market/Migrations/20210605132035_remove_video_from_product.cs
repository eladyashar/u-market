using Microsoft.EntityFrameworkCore.Migrations;

namespace u_market.Migrations
{
    public partial class remove_video_from_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "video_url",
                table: "products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "video_url",
                table: "products",
                type: "text",
                nullable: true);
        }
    }
}

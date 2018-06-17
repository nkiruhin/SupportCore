using Microsoft.EntityFrameworkCore.Migrations;

namespace SupportCore.Migrations
{
    public partial class apikey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "Person",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Configuration",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Configuration",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Configuration");
        }
    }
}

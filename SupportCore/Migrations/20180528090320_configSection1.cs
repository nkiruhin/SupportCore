using Microsoft.EntityFrameworkCore.Migrations;

namespace SupportCore.Migrations
{
    public partial class configSection1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Section",
                table: "Configuration",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Section",
                table: "Configuration");
        }
    }
}

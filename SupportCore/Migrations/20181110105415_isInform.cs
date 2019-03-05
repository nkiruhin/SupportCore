using Microsoft.EntityFrameworkCore.Migrations;

namespace SupportCore.Migrations
{
    public partial class isInform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInform",
                table: "TicketThreads",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Configuration",
                keyColumn: "Name",
                keyValue: "FromAddress");

            migrationBuilder.DeleteData(
                table: "Configuration",
                keyColumn: "Name",
                keyValue: "FromName");

            migrationBuilder.DeleteData(
                table: "Configuration",
                keyColumn: "Name",
                keyValue: "Signature");

            migrationBuilder.DeleteData(
                table: "Configuration",
                keyColumn: "Name",
                keyValue: "UserId");

            migrationBuilder.DeleteData(
                table: "Configuration",
                keyColumn: "Name",
                keyValue: "UserPassword");

            migrationBuilder.DropColumn(
                name: "IsInform",
                table: "TicketThreads");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SupportCore.Migrations
{
    public partial class index_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tickets_DateCreate",
                table: "Tickets",
                column: "DateCreate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_DateCreate",
                table: "Tickets");
        }
    }
}

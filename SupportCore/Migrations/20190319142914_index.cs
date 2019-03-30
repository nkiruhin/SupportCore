using Microsoft.EntityFrameworkCore.Migrations;

namespace SupportCore.Migrations
{
    public partial class index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "Fields",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fields_Label",
                table: "Fields",
                column: "Label");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Fields_Label",
                table: "Fields");

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "Fields",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

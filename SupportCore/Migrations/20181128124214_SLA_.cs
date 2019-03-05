using Microsoft.EntityFrameworkCore.Migrations;

namespace SupportCore.Migrations
{
    public partial class SLA_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeOff",
                table: "SLAs",
                newName: "Type");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "SLAs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "SLAs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResponseTime",
                table: "SLAs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Time",
                table: "SLAs",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ResponseTime", "Type" },
                values: new object[] { 48, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_SLAs_CategoryId",
                table: "SLAs",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SLAs_Category_CategoryId",
                table: "SLAs",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SLAs_Category_CategoryId",
                table: "SLAs");

            migrationBuilder.DropIndex(
                name: "IX_SLAs_CategoryId",
                table: "SLAs");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "SLAs");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "SLAs");

            migrationBuilder.DropColumn(
                name: "ResponseTime",
                table: "SLAs");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "SLAs");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "SLAs",
                newName: "TimeOff");

            migrationBuilder.UpdateData(
                table: "SLAs",
                keyColumn: "Id",
                keyValue: 1,
                column: "TimeOff",
                value: 48);
        }
    }
}

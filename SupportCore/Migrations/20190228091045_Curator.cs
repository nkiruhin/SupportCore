using Microsoft.EntityFrameworkCore.Migrations;

namespace SupportCore.Migrations
{
    public partial class Curator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CuratorId",
                table: "Organizations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CuratorId",
                table: "Organizations",
                column: "CuratorId",
                unique: true,
                filter: "[CuratorId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Person_CuratorId",
                table: "Organizations",
                column: "CuratorId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Person_CuratorId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_CuratorId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CuratorId",
                table: "Organizations");
        }
    }
}

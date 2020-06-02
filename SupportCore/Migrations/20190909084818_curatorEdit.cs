using Microsoft.EntityFrameworkCore.Migrations;

namespace SupportCore.Migrations
{
    public partial class curatorEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_Organizations_OrganizationId",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Person_OrganizationId",
                table: "Person");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Person_OrganizationId",
                table: "Person",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Organizations_OrganizationId",
                table: "Person",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

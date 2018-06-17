using Microsoft.EntityFrameworkCore.Migrations;

namespace SupportCore.Migrations
{
    public partial class provider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<bool>(
                name: "IsInform",
                table: "Tickets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isProvider",
                table: "Organizations",
                nullable: false,
                defaultValue: false);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
 
            migrationBuilder.DropColumn(
                name: "IsInform",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "isProvider",
                table: "Organizations");

            migrationBuilder.AlterColumn<string>(
                name: "PersonId",
                table: "Tickets",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Person_PersonId",
                table: "Tickets",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

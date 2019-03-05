using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SupportCore.Migrations
{
    public partial class SLA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SLAId",
                table: "Organizations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SLAs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IsDefault = table.Column<int>(nullable: false, defaultValue: 0),
                    TimeOff = table.Column<int>(nullable: false),
                    FieldId = table.Column<int>(nullable: true),
                    FieldValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SLAs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SLAs_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "SLAs",
                columns: new[] { "Id", "FieldId", "FieldValue", "IsDefault", "Name", "TimeOff" },
                values: new object[] { 1, null, null, 1, "Базовый SLA", 48 });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_SLAId",
                table: "Organizations",
                column: "SLAId");

            migrationBuilder.CreateIndex(
                name: "IX_SLAs_FieldId",
                table: "SLAs",
                column: "FieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_SLAs_SLAId",
                table: "Organizations",
                column: "SLAId",
                principalTable: "SLAs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_SLAs_SLAId",
                table: "Organizations");

            migrationBuilder.DropTable(
                name: "SLAs");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_SLAId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "SLAId",
                table: "Organizations");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoGraphic.Migrations
{
    public partial class nnnnnnn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AspNetUsers_identityUserId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_identityUserId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "identityUserId",
                table: "Departments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "identityUserId",
                table: "Departments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_identityUserId",
                table: "Departments",
                column: "identityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_AspNetUsers_identityUserId",
                table: "Departments",
                column: "identityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

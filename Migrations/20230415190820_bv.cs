using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoGraphic.Migrations
{
    public partial class bv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AspNetUsers_ApplicationUserId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_ApplicationUserId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "NumberOfUsers",
                table: "Departments");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId1",
                table: "AspNetUsers",
                column: "DepartmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Departments_DepartmentId1",
                table: "AspNetUsers",
                column: "DepartmentId1",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Departments_DepartmentId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DepartmentId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Departments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfUsers",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ApplicationUserId",
                table: "Departments",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_AspNetUsers_ApplicationUserId",
                table: "Departments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

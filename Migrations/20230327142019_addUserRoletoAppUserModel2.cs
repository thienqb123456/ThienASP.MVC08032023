using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThienASPMVC08032023.Migrations
{
    public partial class addUserRoletoAppUserModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_UserRoleId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserRoleId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_UserRoleId",
                table: "AspNetUsers",
                column: "UserRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_UserRoleId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserRoleId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_UserRoleId",
                table: "AspNetUsers",
                column: "UserRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

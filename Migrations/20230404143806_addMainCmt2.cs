using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThienASPMVC08032023.Migrations
{
    public partial class addMainCmt2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MainComments_Clips_ClipId",
                table: "MainComments");

            migrationBuilder.AlterColumn<int>(
                name: "ClipId",
                table: "MainComments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MainComments_Clips_ClipId",
                table: "MainComments",
                column: "ClipId",
                principalTable: "Clips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MainComments_Clips_ClipId",
                table: "MainComments");

            migrationBuilder.AlterColumn<int>(
                name: "ClipId",
                table: "MainComments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_MainComments_Clips_ClipId",
                table: "MainComments",
                column: "ClipId",
                principalTable: "Clips",
                principalColumn: "Id");
        }
    }
}

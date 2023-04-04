using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThienASPMVC08032023.Migrations
{
    public partial class addCommentModeltoDb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClipId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ClipId",
                table: "Comments",
                column: "ClipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Clips_ClipId",
                table: "Comments",
                column: "ClipId",
                principalTable: "Clips",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Clips_ClipId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ClipId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ClipId",
                table: "Comments");
        }
    }
}

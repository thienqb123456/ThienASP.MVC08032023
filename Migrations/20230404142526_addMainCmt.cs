using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThienASPMVC08032023.Migrations
{
    public partial class addMainCmt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClipId = table.Column<int>(type: "int", nullable: true),
                    CommentMsg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainComments_Clips_ClipId",
                        column: x => x.ClipId,
                        principalTable: "Clips",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MainComments_ClipId",
                table: "MainComments",
                column: "ClipId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainComments");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThienASPMVC08032023.Migrations
{
    public partial class AddAuthorUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorUsername",
                table: "Clips",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorUsername",
                table: "Clips");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThienASPMVC08032023.Migrations
{
    /// <inheritdoc />
    public partial class addUrltoClipModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Clips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Clips");
        }
    }
}

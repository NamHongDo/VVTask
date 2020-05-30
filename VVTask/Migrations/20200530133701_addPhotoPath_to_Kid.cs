using Microsoft.EntityFrameworkCore.Migrations;

namespace VVTask.Migrations
{
    public partial class addPhotoPath_to_Kid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Kids",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Kids");
        }
    }
}

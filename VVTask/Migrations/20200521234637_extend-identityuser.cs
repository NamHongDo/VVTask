using Microsoft.EntityFrameworkCore.Migrations;

namespace VVTask.Migrations
{
    public partial class extendidentityuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Kids",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kids_ApplicationUserId",
                table: "Kids",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kids_AspNetUsers_ApplicationUserId",
                table: "Kids",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kids_AspNetUsers_ApplicationUserId",
                table: "Kids");

            migrationBuilder.DropIndex(
                name: "IX_Kids_ApplicationUserId",
                table: "Kids");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Kids");
        }
    }
}

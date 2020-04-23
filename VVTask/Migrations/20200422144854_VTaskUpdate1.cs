using Microsoft.EntityFrameworkCore.Migrations;

namespace VVTask.Migrations
{
    public partial class VTaskUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VTasks_Categories_CategoryId",
                table: "VTasks");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VTasks",
                table: "VTasks");

            migrationBuilder.DropIndex(
                name: "IX_VTasks_CategoryId",
                table: "VTasks");

            migrationBuilder.DropColumn(
                name: "VTaskId",
                table: "VTasks");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "VTasks");

            migrationBuilder.DropColumn(
                name: "IsTaskOfTheWeek",
                table: "VTasks");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "VTasks");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VTasks",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "VType",
                table: "VTasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VTasks",
                table: "VTasks",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VTasks",
                table: "VTasks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VTasks");

            migrationBuilder.DropColumn(
                name: "VType",
                table: "VTasks");

            migrationBuilder.AddColumn<int>(
                name: "VTaskId",
                table: "VTasks",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "VTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsTaskOfTheWeek",
                table: "VTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "VTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VTasks",
                table: "VTasks",
                column: "VTaskId");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VTasks_CategoryId",
                table: "VTasks",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_VTasks_Categories_CategoryId",
                table: "VTasks",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

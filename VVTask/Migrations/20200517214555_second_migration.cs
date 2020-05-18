using Microsoft.EntityFrameworkCore.Migrations;

namespace VVTask.Migrations
{
    public partial class second_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VTasks_Kids_KidId",
                table: "VTasks");

            migrationBuilder.AlterColumn<int>(
                name: "KidId",
                table: "VTasks",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VTasks_Kids_KidId",
                table: "VTasks",
                column: "KidId",
                principalTable: "Kids",
                principalColumn: "KidId",
                onUpdate: ReferentialAction.Cascade,
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VTasks_Kids_KidId",
                table: "VTasks");

            migrationBuilder.AlterColumn<int>(
                name: "KidId",
                table: "VTasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_VTasks_Kids_KidId",
                table: "VTasks",
                column: "KidId",
                principalTable: "Kids",
                principalColumn: "KidId",
                onUpdate: ReferentialAction.Cascade,
                onDelete: ReferentialAction.Restrict);
        }
    }
}

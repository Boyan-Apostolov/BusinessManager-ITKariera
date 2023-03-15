using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VocationManager.Data.Migrations
{
    public partial class Projects_AddPriority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Projects");
        }
    }
}

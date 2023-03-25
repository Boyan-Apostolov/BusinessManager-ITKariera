using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VocationManager.Data.Migrations
{
    public partial class TimeOffs_ChangeTypeProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeOffs_TimeOffs_TypeId",
                table: "TimeOffs");

            migrationBuilder.DropIndex(
                name: "IX_TimeOffs_TypeId",
                table: "TimeOffs");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "TimeOffs",
                newName: "Type");

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "TimeOffs",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "TimeOffs",
                newName: "TypeId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "TimeOffs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeOffs_TypeId",
                table: "TimeOffs",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeOffs_TimeOffs_TypeId",
                table: "TimeOffs",
                column: "TypeId",
                principalTable: "TimeOffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

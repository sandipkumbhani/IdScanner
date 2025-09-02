using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdScanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IdScanner_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDatas_Departments_DepartmentId",
                table: "UserDatas");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Departments",
                newName: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDatas_Departments_DepartmentId",
                table: "UserDatas",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.NoAction);
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDatas_Departments_DepartmentId",
                table: "UserDatas");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Departments",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDatas_Departments_DepartmentId",
                table: "UserDatas",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocPass.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addAllowData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AllowNoOfContact",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AllowNoOfEmail",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AllowNoOfName",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SocietyDataId",
                table: "Subscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SocietyDataId",
                table: "Subscriptions",
                column: "SocietyDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_SocietyData_SocietyDataId",
                table: "Subscriptions",
                column: "SocietyDataId",
                principalTable: "SocietyData",
                principalColumn: "SocietyDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SocietyData_SocietyDataId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_SocietyDataId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "AllowNoOfContact",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "AllowNoOfEmail",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "AllowNoOfName",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SocietyDataId",
                table: "Subscriptions");
        }
    }
}

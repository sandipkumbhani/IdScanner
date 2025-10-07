using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocPass.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatekey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SocietyData_SocietyDataId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_SocietyDataId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SocietyDataId",
                table: "Subscriptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}

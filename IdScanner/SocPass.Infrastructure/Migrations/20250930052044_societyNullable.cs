using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocPass.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class societyNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_societies_SocietyId",
                table: "Subscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "SocietyId",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_societies_SocietyId",
                table: "Subscriptions",
                column: "SocietyId",
                principalTable: "societies",
                principalColumn: "SocietyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_societies_SocietyId",
                table: "Subscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "SocietyId",
                table: "Subscriptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_societies_SocietyId",
                table: "Subscriptions",
                column: "SocietyId",
                principalTable: "societies",
                principalColumn: "SocietyId");
        }
    }
}

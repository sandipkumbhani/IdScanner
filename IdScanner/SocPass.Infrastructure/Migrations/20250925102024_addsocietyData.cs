using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocPass.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addsocietyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_users_UserId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_users_societies_SocietyId",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Subscriptions",
                newName: "SocietyId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_SocietyId");

            migrationBuilder.AlterColumn<int>(
                name: "SocietyId",
                table: "users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_societies_SocietyId",
                table: "Subscriptions",
                column: "SocietyId",
                principalTable: "societies",
                principalColumn: "SocietyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_societies_SocietyId",
                table: "users",
                column: "SocietyId",
                principalTable: "societies",
                principalColumn: "SocietyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_societies_SocietyId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_users_societies_SocietyId",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "SocietyId",
                table: "Subscriptions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_SocietyId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "SocietyId",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_users_UserId",
                table: "Subscriptions",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_societies_SocietyId",
                table: "users",
                column: "SocietyId",
                principalTable: "societies",
                principalColumn: "SocietyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

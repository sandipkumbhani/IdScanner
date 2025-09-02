using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdScanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AadhaarCardNumber",
                table: "UserDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BloodGroup",
                table: "UserDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QRCodeText",
                table: "UserDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignatureUrl",
                table: "UserDatas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AadhaarCardNumber",
                table: "UserDatas");

            migrationBuilder.DropColumn(
                name: "BloodGroup",
                table: "UserDatas");

            migrationBuilder.DropColumn(
                name: "QRCodeText",
                table: "UserDatas");

            migrationBuilder.DropColumn(
                name: "SignatureUrl",
                table: "UserDatas");
        }
    }
}

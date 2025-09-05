using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdScanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IdScanner_second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QRCodeText",
                table: "UserDatas",
                newName: "SignatureFileName");

            migrationBuilder.AddColumn<string>(
                name: "MedicalCertificateFileName",
                table: "UserDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoFileName",
                table: "UserDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PoliceVerificationCertificateFileName",
                table: "UserDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QRCodeUrl",
                table: "UserDatas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicalCertificateFileName",
                table: "UserDatas");

            migrationBuilder.DropColumn(
                name: "PhotoFileName",
                table: "UserDatas");

            migrationBuilder.DropColumn(
                name: "PoliceVerificationCertificateFileName",
                table: "UserDatas");

            migrationBuilder.DropColumn(
                name: "QRCodeUrl",
                table: "UserDatas");

            migrationBuilder.RenameColumn(
                name: "SignatureFileName",
                table: "UserDatas",
                newName: "QRCodeText");
        }
    }
}

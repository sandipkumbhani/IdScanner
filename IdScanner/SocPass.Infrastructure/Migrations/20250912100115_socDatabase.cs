using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocPass.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class socDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FloorNumber",
                table: "flats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "InsertBy",
                table: "blocks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                table: "blocks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "blocks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "UpdateBy",
                table: "blocks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "blocks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FloorNumber",
                table: "flats");

            migrationBuilder.DropColumn(
                name: "InsertBy",
                table: "blocks");

            migrationBuilder.DropColumn(
                name: "InsertDate",
                table: "blocks");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "blocks");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "blocks");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "blocks");
        }
    }
}

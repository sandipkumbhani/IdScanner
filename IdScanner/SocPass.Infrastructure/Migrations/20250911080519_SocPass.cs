using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocPass.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SocPass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmailId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InsertBy = table.Column<long>(type: "bigint", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "societies",
                columns: table => new
                {
                    SocietyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InsertBy = table.Column<long>(type: "bigint", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_societies", x => x.SocietyId);
                    table.ForeignKey(
                        name: "FK_societies_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blocks",
                columns: table => new
                {
                    BlockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SocietyId = table.Column<int>(type: "int", nullable: false),
                    BlockNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blocks", x => x.BlockId);
                    table.ForeignKey(
                        name: "FK_blocks_societies_SocietyId",
                        column: x => x.SocietyId,
                        principalTable: "societies",
                        principalColumn: "SocietyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "flats",
                columns: table => new
                {
                    FlatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SocietyId = table.Column<int>(type: "int", nullable: false),
                    BlockId = table.Column<int>(type: "int", nullable: false),
                    NumberOfFlats = table.Column<int>(type: "int", nullable: false),
                    FlatNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalMember = table.Column<int>(type: "int", nullable: false),
                    NumberOfAdult = table.Column<int>(type: "int", nullable: false),
                    NumberOfChild = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InsertBy = table.Column<long>(type: "bigint", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flats", x => x.FlatId);
                    table.ForeignKey(
                        name: "FK_flats_blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "blocks",
                        principalColumn: "BlockId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_flats_societies_SocietyId",
                        column: x => x.SocietyId,
                        principalTable: "societies",
                        principalColumn: "SocietyId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "members",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlatId = table.Column<int>(type: "int", nullable: false),
                    IsChild = table.Column<bool>(type: "bit", nullable: false),
                    ChildAge = table.Column<int>(type: "int", nullable: false),
                    QRCodeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visited = table.Column<bool>(type: "bit", nullable: false),
                    PassDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsGuest = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InsertBy = table.Column<long>(type: "bigint", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_members", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_members_flats_FlatId",
                        column: x => x.FlatId,
                        principalTable: "flats",
                        principalColumn: "FlatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_blocks_SocietyId",
                table: "blocks",
                column: "SocietyId");

            migrationBuilder.CreateIndex(
                name: "IX_flats_BlockId",
                table: "flats",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_flats_SocietyId",
                table: "flats",
                column: "SocietyId");

            migrationBuilder.CreateIndex(
                name: "IX_members_FlatId",
                table: "members",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_societies_UserId",
                table: "societies",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "members");

            migrationBuilder.DropTable(
                name: "flats");

            migrationBuilder.DropTable(
                name: "blocks");

            migrationBuilder.DropTable(
                name: "societies");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

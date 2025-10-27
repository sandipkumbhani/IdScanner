using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocPass.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addEventTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "MenuMasters",
            //    columns: table => new
            //    {
            //        MenuId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
            //        Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsDefault = table.Column<bool>(type: "bit", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        InsertBy = table.Column<long>(type: "bigint", nullable: false),
            //        InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdateBy = table.Column<long>(type: "bigint", nullable: false),
            //        UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MenuMasters", x => x.MenuId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Roles",
            //    columns: table => new
            //    {
            //        UserRoleId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        InsertBy = table.Column<long>(type: "bigint", nullable: false),
            //        InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdateBy = table.Column<long>(type: "bigint", nullable: false),
            //        UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Roles", x => x.UserRoleId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "societies",
            //    columns: table => new
            //    {
            //        SocietyId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Contact2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        InsertBy = table.Column<long>(type: "bigint", nullable: false),
            //        InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdateBy = table.Column<long>(type: "bigint", nullable: false),
            //        UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_societies", x => x.SocietyId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "blocks",
            //    columns: table => new
            //    {
            //        BlockId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SocietyId = table.Column<int>(type: "int", nullable: false),
            //        BlockNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        InsertBy = table.Column<long>(type: "bigint", nullable: false),
            //        InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdateBy = table.Column<long>(type: "bigint", nullable: false),
            //        UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_blocks", x => x.BlockId);
            //        table.ForeignKey(
            //            name: "FK_blocks_societies_SocietyId",
            //            column: x => x.SocietyId,
            //            principalTable: "societies",
            //            principalColumn: "SocietyId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Organizer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InsertBy = table.Column<long>(type: "bigint", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SocietyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Events_societies_SocietyId",
                        column: x => x.SocietyId,
                        principalTable: "societies",
                        principalColumn: "SocietyId");
                });

            //migrationBuilder.CreateTable(
            //    name: "Subscriptions",
            //    columns: table => new
            //    {
            //        SubscriptionId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SocietyId = table.Column<int>(type: "int", nullable: false),
            //        StartFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        EndTo = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        AllowNoOfName = table.Column<int>(type: "int", nullable: false),
            //        AllowNoOfContact = table.Column<int>(type: "int", nullable: false),
            //        AllowNoOfEmail = table.Column<int>(type: "int", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        InsertBy = table.Column<long>(type: "bigint", nullable: false),
            //        InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdateBy = table.Column<long>(type: "bigint", nullable: false),
            //        UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
            //        table.ForeignKey(
            //            name: "FK_Subscriptions_societies_SocietyId",
            //            column: x => x.SocietyId,
            //            principalTable: "societies",
            //            principalColumn: "SocietyId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "users",
            //    columns: table => new
            //    {
            //        UserId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SocietyId = table.Column<int>(type: "int", nullable: true),
            //        UserRoleId = table.Column<int>(type: "int", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
            //        EmailId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        Password = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        InsertBy = table.Column<long>(type: "bigint", nullable: false),
            //        InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdateBy = table.Column<long>(type: "bigint", nullable: false),
            //        UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_users", x => x.UserId);
            //        table.ForeignKey(
            //            name: "FK_users_Roles_UserRoleId",
            //            column: x => x.UserRoleId,
            //            principalTable: "Roles",
            //            principalColumn: "UserRoleId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_users_societies_SocietyId",
            //            column: x => x.SocietyId,
            //            principalTable: "societies",
            //            principalColumn: "SocietyId");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "flats",
            //    columns: table => new
            //    {
            //        FlatId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SocietyId = table.Column<int>(type: "int", nullable: false),
            //        BlockId = table.Column<int>(type: "int", nullable: false),
            //        NumberOfFlats = table.Column<int>(type: "int", nullable: false),
            //        FlatNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FloorNumber = table.Column<int>(type: "int", nullable: false),
            //        TotalMember = table.Column<int>(type: "int", nullable: false),
            //        NumberOfAdult = table.Column<int>(type: "int", nullable: false),
            //        NumberOfChild = table.Column<int>(type: "int", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        InsertBy = table.Column<long>(type: "bigint", nullable: false),
            //        InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdateBy = table.Column<long>(type: "bigint", nullable: false),
            //        UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_flats", x => x.FlatId);
            //        table.ForeignKey(
            //            name: "FK_flats_blocks_BlockId",
            //            column: x => x.BlockId,
            //            principalTable: "blocks",
            //            principalColumn: "BlockId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_flats_societies_SocietyId",
            //            column: x => x.SocietyId,
            //            principalTable: "societies",
            //            principalColumn: "SocietyId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "members",
            //    columns: table => new
            //    {
            //        MemberId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FlatId = table.Column<int>(type: "int", nullable: false),
            //        IsChild = table.Column<bool>(type: "bit", nullable: false),
            //        ChildAge = table.Column<int>(type: "int", nullable: false),
            //        QRCodeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Visited = table.Column<bool>(type: "bit", nullable: false),
            //        PassDate = table.Column<DateOnly>(type: "date", nullable: false),
            //        IsGuest = table.Column<bool>(type: "bit", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        InsertBy = table.Column<long>(type: "bigint", nullable: false),
            //        InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdateBy = table.Column<long>(type: "bigint", nullable: false),
            //        UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_members", x => x.MemberId);
            //        table.ForeignKey(
            //            name: "FK_members_flats_FlatId",
            //            column: x => x.FlatId,
            //            principalTable: "flats",
            //            principalColumn: "FlatId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SocietyData",
            //    columns: table => new
            //    {
            //        SocietyDataId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FlatId = table.Column<int>(type: "int", nullable: false),
            //        ContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        InsertBy = table.Column<long>(type: "bigint", nullable: false),
            //        InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdateBy = table.Column<long>(type: "bigint", nullable: false),
            //        UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SocietyData", x => x.SocietyDataId);
            //        table.ForeignKey(
            //            name: "FK_SocietyData_flats_FlatId",
            //            column: x => x.FlatId,
            //            principalTable: "flats",
            //            principalColumn: "FlatId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "userFlatMappings",
            //    columns: table => new
            //    {
            //        UserFlatMappingId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserId = table.Column<int>(type: "int", nullable: false),
            //        FlatId = table.Column<int>(type: "int", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        InsertBy = table.Column<long>(type: "bigint", nullable: false),
            //        InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdateBy = table.Column<long>(type: "bigint", nullable: false),
            //        UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_userFlatMappings", x => x.UserFlatMappingId);
            //        table.ForeignKey(
            //            name: "FK_userFlatMappings_flats_FlatId",
            //            column: x => x.FlatId,
            //            principalTable: "flats",
            //            principalColumn: "FlatId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_blocks_SocietyId",
            //    table: "blocks",
            //    column: "SocietyId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_SocietyId",
                table: "Events",
                column: "SocietyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_flats_BlockId",
            //    table: "flats",
            //    column: "BlockId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_flats_SocietyId",
            //    table: "flats",
            //    column: "SocietyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_members_FlatId",
            //    table: "members",
            //    column: "FlatId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SocietyData_FlatId",
            //    table: "SocietyData",
            //    column: "FlatId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Subscriptions_SocietyId",
            //    table: "Subscriptions",
            //    column: "SocietyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_userFlatMappings_FlatId",
            //    table: "userFlatMappings",
            //    column: "FlatId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_users_SocietyId",
            //    table: "users",
            //    column: "SocietyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_users_UserRoleId",
            //    table: "users",
            //    column: "UserRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            //migrationBuilder.DropTable(
            //    name: "members");

            //migrationBuilder.DropTable(
            //    name: "MenuMasters");

            //migrationBuilder.DropTable(
            //    name: "SocietyData");

            //migrationBuilder.DropTable(
            //    name: "Subscriptions");

            //migrationBuilder.DropTable(
            //    name: "userFlatMappings");

            //migrationBuilder.DropTable(
            //    name: "users");

            //migrationBuilder.DropTable(
            //    name: "flats");

            //migrationBuilder.DropTable(
            //    name: "Roles");

            //migrationBuilder.DropTable(
            //    name: "blocks");

            //migrationBuilder.DropTable(
            //    name: "societies");
        }
    }
}

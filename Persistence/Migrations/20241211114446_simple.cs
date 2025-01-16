using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class simple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d7ccb8fe-4413-410f-a4cc-ff182aac82a0");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4f26f166-c32d-42d3-8e70-da13e407444e", "ac1d2eac-7cf6-44c4-b355-6140c1eedede" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f26f166-c32d-42d3-8e70-da13e407444e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ac1d2eac-7cf6-44c4-b355-6140c1eedede");

            migrationBuilder.CreateTable(
                name: "TrackerLogs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionType = table.Column<int>(type: "int", nullable: true),
                    Page = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackerLogs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1c626251-4287-446c-8d4b-dd26cd967e73", "0eab7e50-fdba-425b-aa5f-e0c8826a55bc", "Customer", "CUSTOMER" },
                    { "3047ce9b-78a6-44e9-8e04-c7972a68fa77", "5adc45a6-587b-423e-a2f1-82f768144ac6", "Master", "MASTER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreationDate", "DeleteDate", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdateDate", "UserName", "WhoAdded" },
                values: new object[] { "471a7c07-5b3b-42f4-81d1-b758f57ef1a3", 0, "525eaf71-992c-4224-821b-75f01cdd7449", new DateTime(2024, 12, 11, 11, 44, 45, 819, DateTimeKind.Utc).AddTicks(2415), null, "test@test.com", true, "admin", false, null, "TEST@TEST.COM", "ADMIN", "AQAAAAEAACcQAAAAEMIvRA61NHINPY1pfBNT5SXrLSS5VKO2YnCF4z2oMTJR/Gu2PLxFRVMjtX39I4apwg==", null, false, "d4e58756-89ac-4f17-bf0f-95948c4ddbfd", 1, false, null, "admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "3047ce9b-78a6-44e9-8e04-c7972a68fa77", "471a7c07-5b3b-42f4-81d1-b758f57ef1a3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackerLogs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c626251-4287-446c-8d4b-dd26cd967e73");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3047ce9b-78a6-44e9-8e04-c7972a68fa77", "471a7c07-5b3b-42f4-81d1-b758f57ef1a3" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3047ce9b-78a6-44e9-8e04-c7972a68fa77");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "471a7c07-5b3b-42f4-81d1-b758f57ef1a3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4f26f166-c32d-42d3-8e70-da13e407444e", "45ef68d6-d201-43f9-8f9f-788d8761d025", "Master", "MASTER" },
                    { "d7ccb8fe-4413-410f-a4cc-ff182aac82a0", "64006622-f95e-49f6-a4ae-820d6a8e9eb2", "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreationDate", "DeleteDate", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdateDate", "UserName", "WhoAdded" },
                values: new object[] { "ac1d2eac-7cf6-44c4-b355-6140c1eedede", 0, "ddcbb110-d2fd-4bc3-8d36-9ab638ebc098", new DateTime(2024, 12, 4, 10, 30, 31, 982, DateTimeKind.Utc).AddTicks(5930), null, "test@test.com", true, "admin", false, null, "TEST@TEST.COM", "ADMIN", "AQAAAAEAACcQAAAAEMIvRA61NHINPY1pfBNT5SXrLSS5VKO2YnCF4z2oMTJR/Gu2PLxFRVMjtX39I4apwg==", null, false, "6784c4ae-15fe-4d9c-bf00-23d546a5de5a", 1, false, null, "admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "4f26f166-c32d-42d3-8e70-da13e407444e", "ac1d2eac-7cf6-44c4-b355-6140c1eedede" });
        }
    }
}

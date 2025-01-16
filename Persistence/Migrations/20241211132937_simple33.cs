using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class simple33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "TrackerLogs",
                newName: "TargetEmail");

            migrationBuilder.AddColumn<string>(
                name: "LoggedInUserEmail",
                table: "TrackerLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3bc4965e-6862-44a5-981f-d5f537352883", "2eb11bea-9039-4bec-b81b-fc48f6e36811", "Master", "MASTER" },
                    { "b4ceb354-5a87-4af4-a67b-550a91917bcf", "37795ffb-8c53-40ca-98ae-3f42f74d058c", "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreationDate", "DeleteDate", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdateDate", "UserName", "WhoAdded" },
                values: new object[] { "4cbd8def-cd24-4466-baec-3273a6798d66", 0, "e78915a8-71c3-41e6-a728-70c751aeb0b1", new DateTime(2024, 12, 11, 13, 29, 37, 111, DateTimeKind.Utc).AddTicks(845), null, "test@test.com", true, "admin", false, null, "TEST@TEST.COM", "ADMIN", "AQAAAAEAACcQAAAAEMIvRA61NHINPY1pfBNT5SXrLSS5VKO2YnCF4z2oMTJR/Gu2PLxFRVMjtX39I4apwg==", null, false, "4f16bbdd-9cc7-4028-88c4-7c48c7fccf7f", 1, false, null, "admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "3bc4965e-6862-44a5-981f-d5f537352883", "4cbd8def-cd24-4466-baec-3273a6798d66" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4ceb354-5a87-4af4-a67b-550a91917bcf");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3bc4965e-6862-44a5-981f-d5f537352883", "4cbd8def-cd24-4466-baec-3273a6798d66" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3bc4965e-6862-44a5-981f-d5f537352883");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4cbd8def-cd24-4466-baec-3273a6798d66");

            migrationBuilder.DropColumn(
                name: "LoggedInUserEmail",
                table: "TrackerLogs");

            migrationBuilder.RenameColumn(
                name: "TargetEmail",
                table: "TrackerLogs",
                newName: "Email");

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
    }
}

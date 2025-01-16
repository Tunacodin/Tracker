using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class simple34 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "TrackerLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "40427079-3eb7-40ae-88c2-b2ca80648809", "91e8fa93-dec7-4b29-a93d-cf625df54526", "Customer", "CUSTOMER" },
                    { "bff3fbe6-5bb7-41b2-b501-268afa3c4b66", "1954bec2-a391-4570-9e41-7763ae78a410", "Master", "MASTER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreationDate", "DeleteDate", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdateDate", "UserName", "WhoAdded" },
                values: new object[] { "b14bfe05-9232-4406-8463-02e1c2ee32ef", 0, "c345c8b6-660b-4790-bcf0-e5a66d33995a", new DateTime(2024, 12, 11, 17, 2, 4, 175, DateTimeKind.Utc).AddTicks(7920), null, "test@test.com", true, "admin", false, null, "TEST@TEST.COM", "ADMIN", "AQAAAAEAACcQAAAAEMIvRA61NHINPY1pfBNT5SXrLSS5VKO2YnCF4z2oMTJR/Gu2PLxFRVMjtX39I4apwg==", null, false, "22d33446-6bab-48fc-bd81-8fb6ce0ea09b", 1, false, null, "admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "bff3fbe6-5bb7-41b2-b501-268afa3c4b66", "b14bfe05-9232-4406-8463-02e1c2ee32ef" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40427079-3eb7-40ae-88c2-b2ca80648809");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "bff3fbe6-5bb7-41b2-b501-268afa3c4b66", "b14bfe05-9232-4406-8463-02e1c2ee32ef" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bff3fbe6-5bb7-41b2-b501-268afa3c4b66");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b14bfe05-9232-4406-8463-02e1c2ee32ef");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "TrackerLogs");

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
    }
}

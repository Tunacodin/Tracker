﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class simple35 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "ProcessStatus",
                table: "TrackerLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetAccount",
                table: "TrackerLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "36971528-dab3-4e27-9e02-500b5eb0788a", "9d4d6549-e24d-461c-867f-f3b2e3bd9be4", "Master", "MASTER" },
                    { "cec6af65-6c25-4587-829a-c20761f244e6", "c1c6c20b-2e38-450b-984c-4621654e5db2", "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreationDate", "DeleteDate", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdateDate", "UserName", "WhoAdded" },
                values: new object[] { "e458af5c-d303-4530-9d06-67ba517b486b", 0, "72a64ecc-cf26-40f3-be0c-bf74ba258518", new DateTime(2024, 12, 11, 18, 15, 42, 500, DateTimeKind.Utc).AddTicks(385), null, "test@test.com", true, "admin", false, null, "TEST@TEST.COM", "ADMIN", "AQAAAAEAACcQAAAAEMIvRA61NHINPY1pfBNT5SXrLSS5VKO2YnCF4z2oMTJR/Gu2PLxFRVMjtX39I4apwg==", null, false, "89d42d02-08bd-4e81-a2d9-3bddeabc8f95", 1, false, null, "admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "36971528-dab3-4e27-9e02-500b5eb0788a", "e458af5c-d303-4530-9d06-67ba517b486b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cec6af65-6c25-4587-829a-c20761f244e6");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "36971528-dab3-4e27-9e02-500b5eb0788a", "e458af5c-d303-4530-9d06-67ba517b486b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "36971528-dab3-4e27-9e02-500b5eb0788a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e458af5c-d303-4530-9d06-67ba517b486b");

            migrationBuilder.DropColumn(
                name: "ProcessStatus",
                table: "TrackerLogs");

            migrationBuilder.DropColumn(
                name: "TargetAccount",
                table: "TrackerLogs");

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
    }
}

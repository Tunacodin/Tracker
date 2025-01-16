using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13a5a24b-7129-40d1-bbcc-6c867446b3ec");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ba87795e-5327-4fc4-94f0-db3b9882ffcb", "6f14000d-f470-4d2f-9386-cce9ef47dc1a" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba87795e-5327-4fc4-94f0-db3b9882ffcb");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6f14000d-f470-4d2f-9386-cce9ef47dc1a");

            migrationBuilder.AddColumn<string>(
                name: "Iban",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MypayzNo",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iban",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "MypayzNo",
                table: "Accounts");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "13a5a24b-7129-40d1-bbcc-6c867446b3ec", "b34949d0-4034-47c8-88b2-f5eb84b618e5", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ba87795e-5327-4fc4-94f0-db3b9882ffcb", "8fcce8bc-8623-4817-9cc8-e9b5456c922b", "Master", "MASTER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreationDate", "DeleteDate", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdateDate", "UserName", "WhoAdded" },
                values: new object[] { "6f14000d-f470-4d2f-9386-cce9ef47dc1a", 0, "9f42d9c2-fa24-44d7-9c1e-3c5f0a52840b", new DateTime(2024, 11, 27, 7, 15, 7, 25, DateTimeKind.Utc).AddTicks(959), null, "test@test.com", true, "admin", false, null, "TEST@TEST.COM", "ADMIN", "AQAAAAEAACcQAAAAEMIvRA61NHINPY1pfBNT5SXrLSS5VKO2YnCF4z2oMTJR/Gu2PLxFRVMjtX39I4apwg==", null, false, "9536242a-c3d0-4dca-9de6-e12313250a8f", 1, false, null, "admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ba87795e-5327-4fc4-94f0-db3b9882ffcb", "6f14000d-f470-4d2f-9386-cce9ef47dc1a" });
        }
    }
}

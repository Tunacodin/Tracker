using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class newone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "23afde8c-ca22-4a8f-964f-c6c8ee884a3d", "a7522bf6-1a36-40f7-bd47-6c45ab8f7b70", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "efcf45f5-7e39-4b63-8f81-d3c784777b93", "3e0e199b-89cc-4660-b85c-2933520f979f", "Master", "MASTER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreationDate", "DeleteDate", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdateDate", "UserName", "WhoAdded" },
                values: new object[] { "0d0f75a2-71cc-4061-bf65-3e70e02f3d6e", 0, "019d476a-0521-4256-a5f8-44349c41d886", new DateTime(2024, 11, 27, 20, 47, 16, 475, DateTimeKind.Utc).AddTicks(3447), null, "test@test.com", true, "admin", false, null, "TEST@TEST.COM", "ADMIN", "AQAAAAEAACcQAAAAEMIvRA61NHINPY1pfBNT5SXrLSS5VKO2YnCF4z2oMTJR/Gu2PLxFRVMjtX39I4apwg==", null, false, "b08cb75b-f884-4f6a-a0fe-c97d886b5091", 1, false, null, "admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "efcf45f5-7e39-4b63-8f81-d3c784777b93", "0d0f75a2-71cc-4061-bf65-3e70e02f3d6e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "23afde8c-ca22-4a8f-964f-c6c8ee884a3d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "efcf45f5-7e39-4b63-8f81-d3c784777b93", "0d0f75a2-71cc-4061-bf65-3e70e02f3d6e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "efcf45f5-7e39-4b63-8f81-d3c784777b93");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0d0f75a2-71cc-4061-bf65-3e70e02f3d6e");
        }
    }
}

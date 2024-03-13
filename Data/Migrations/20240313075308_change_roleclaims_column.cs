using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class change_roleclaims_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_IdentityRole_RoleId",
                table: "RoleClaims");

            migrationBuilder.DropTable(
                name: "IdentityRole");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "54acb932-9f69-405f-992e-345e07b9a648");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "7cd85c2c-447a-4a00-bcf8-0b6b25262a39");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "dfa00d08-db3c-4eb2-99a0-48f8f50f1131");

            migrationBuilder.InsertData(
                table: "Animes",
                columns: new[] { "Id", "CreatedOn", "IsDeleted", "Language", "LastModifiedBy", "LastModifiedOn", "Name", "RatingLevel" },
                values: new object[,]
                {
                    { "88267233-173a-4c98-909e-33d4f70773da", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "NPL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Baruto", 2 },
                    { "afd665f1-a03c-4ccb-aceb-4a3c25b71e3e", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "JPN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Naruto", 5 },
                    { "d02f68d4-1e4c-4428-a730-8d4e4c8b5cbc", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "KOR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "One Punch Man", 4 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_Roles_RoleId",
                table: "RoleClaims",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_Roles_RoleId",
                table: "RoleClaims");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "88267233-173a-4c98-909e-33d4f70773da");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "afd665f1-a03c-4ccb-aceb-4a3c25b71e3e");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "d02f68d4-1e4c-4428-a730-8d4e4c8b5cbc");

            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Animes",
                columns: new[] { "Id", "CreatedOn", "IsDeleted", "Language", "LastModifiedBy", "LastModifiedOn", "Name", "RatingLevel" },
                values: new object[,]
                {
                    { "54acb932-9f69-405f-992e-345e07b9a648", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "KOR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "One Punch Man", 4 },
                    { "7cd85c2c-447a-4a00-bcf8-0b6b25262a39", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "NPL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Baruto", 2 },
                    { "dfa00d08-db3c-4eb2-99a0-48f8f50f1131", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "JPN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Naruto", 5 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_IdentityRole_RoleId",
                table: "RoleClaims",
                column: "RoleId",
                principalTable: "IdentityRole",
                principalColumn: "Id");
        }
    }
}

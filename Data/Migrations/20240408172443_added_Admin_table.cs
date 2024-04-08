using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class added_Admin_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "616ae8db-a5f3-4fef-890f-23eeb7e25c2e");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "79f7558e-cc94-486a-ad47-50fabc072853");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "80911b39-782d-4561-8ad2-cc096facde8b");

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admin_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Animes",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "ImageUrl", "IsDeleted", "Language", "LastModifiedBy", "LastModifiedOn", "Name", "RatingLevel" },
                values: new object[,]
                {
                    { "0bafc21d-4033-4883-b16d-c86793c0f0a5", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "JPN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Naruto", 5 },
                    { "9975b3b7-6af4-417d-80e3-0aa559576dd3", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "NPL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Baruto", 2 },
                    { "fa9f72e4-efcc-4d0c-9be4-21b553b563b9", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "KOR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "One Punch Man", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admin_UserId",
                table: "Admin",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "0bafc21d-4033-4883-b16d-c86793c0f0a5");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "9975b3b7-6af4-417d-80e3-0aa559576dd3");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "fa9f72e4-efcc-4d0c-9be4-21b553b563b9");

            migrationBuilder.InsertData(
                table: "Animes",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "ImageUrl", "IsDeleted", "Language", "LastModifiedBy", "LastModifiedOn", "Name", "RatingLevel" },
                values: new object[,]
                {
                    { "616ae8db-a5f3-4fef-890f-23eeb7e25c2e", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "KOR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "One Punch Man", 4 },
                    { "79f7558e-cc94-486a-ad47-50fabc072853", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "NPL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Baruto", 2 },
                    { "80911b39-782d-4561-8ad2-cc096facde8b", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "JPN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Naruto", 5 }
                });
        }
    }
}

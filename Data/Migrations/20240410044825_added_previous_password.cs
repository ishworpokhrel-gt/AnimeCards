using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class added_previous_password : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "PreviousPassword",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HashPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousPassword", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreviousPassword_Users_UserId",
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
                    { "207994b3-47ee-44cc-9ad1-21eafd11b978", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "KOR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "One Punch Man", 4 },
                    { "7a93be5e-1ccd-4f8c-9109-78d4fedb5c80", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "NPL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Baruto", 2 },
                    { "86fbaae9-b41c-44d8-941e-04da4ca84f23", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "JPN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Naruto", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreviousPassword_UserId",
                table: "PreviousPassword",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreviousPassword");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "207994b3-47ee-44cc-9ad1-21eafd11b978");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "7a93be5e-1ccd-4f8c-9109-78d4fedb5c80");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "86fbaae9-b41c-44d8-941e-04da4ca84f23");

            migrationBuilder.InsertData(
                table: "Animes",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "ImageUrl", "IsDeleted", "Language", "LastModifiedBy", "LastModifiedOn", "Name", "RatingLevel" },
                values: new object[,]
                {
                    { "0bafc21d-4033-4883-b16d-c86793c0f0a5", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "JPN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Naruto", 5 },
                    { "9975b3b7-6af4-417d-80e3-0aa559576dd3", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "NPL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Baruto", 2 },
                    { "fa9f72e4-efcc-4d0c-9be4-21b553b563b9", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "KOR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "One Punch Man", 4 }
                });
        }
    }
}

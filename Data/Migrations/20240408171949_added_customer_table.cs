using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class added_customer_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "20d9c562-2228-4959-8550-7b6621e693b0");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "9c0304d1-fad2-4195-b854-c6198a04af29");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "ca0b1e40-95b1-4e3d-94b8-9f6ff22ce2da");

            migrationBuilder.CreateTable(
                name: "Customer",
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
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Users_UserId",
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
                    { "616ae8db-a5f3-4fef-890f-23eeb7e25c2e", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "KOR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "One Punch Man", 4 },
                    { "79f7558e-cc94-486a-ad47-50fabc072853", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "NPL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Baruto", 2 },
                    { "80911b39-782d-4561-8ad2-cc096facde8b", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "JPN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Naruto", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserId",
                table: "Customer",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer");

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

            migrationBuilder.InsertData(
                table: "Animes",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "ImageUrl", "IsDeleted", "Language", "LastModifiedBy", "LastModifiedOn", "Name", "RatingLevel" },
                values: new object[,]
                {
                    { "20d9c562-2228-4959-8550-7b6621e693b0", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "NPL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Baruto", 2 },
                    { "9c0304d1-fad2-4195-b854-c6198a04af29", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "JPN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Naruto", 5 },
                    { "ca0b1e40-95b1-4e3d-94b8-9f6ff22ce2da", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "KOR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "One Punch Man", 4 }
                });
        }
    }
}

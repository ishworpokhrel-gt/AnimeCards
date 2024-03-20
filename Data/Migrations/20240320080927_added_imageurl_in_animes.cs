using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class added_imageurl_in_animes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Animes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Animes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Animes",
                columns: new[] { "Id", "CreatedOn", "ImageUrl", "IsDeleted", "Language", "LastModifiedBy", "LastModifiedOn", "Name", "RatingLevel" },
                values: new object[,]
                {
                    { "2fed646d-6b0b-4472-9e77-831fcaac141c", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "KOR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "One Punch Man", 4 },
                    { "6a2abec8-954b-4628-830b-17168e99fdc0", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "JPN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Naruto", 5 },
                    { "9903d940-8122-4208-8442-ffb6dc51e70c", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "", false, "NPL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Baruto", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "2fed646d-6b0b-4472-9e77-831fcaac141c");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "6a2abec8-954b-4628-830b-17168e99fdc0");

            migrationBuilder.DeleteData(
                table: "Animes",
                keyColumn: "Id",
                keyValue: "9903d940-8122-4208-8442-ffb6dc51e70c");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Animes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Animes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Animes",
                columns: new[] { "Id", "CreatedOn", "IsDeleted", "Language", "LastModifiedBy", "LastModifiedOn", "Name", "RatingLevel" },
                values: new object[,]
                {
                    { "88267233-173a-4c98-909e-33d4f70773da", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "NPL", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Baruto", 2 },
                    { "afd665f1-a03c-4ccb-aceb-4a3c25b71e3e", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "JPN", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Naruto", 5 },
                    { "d02f68d4-1e4c-4428-a730-8d4e4c8b5cbc", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "KOR", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "One Punch Man", 4 }
                });
        }
    }
}

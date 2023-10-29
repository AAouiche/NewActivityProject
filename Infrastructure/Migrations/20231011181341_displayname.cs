using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class displayname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("a72807cb-1639-4aa1-816d-ed0d8cb0d87a"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("f02482c1-26c3-471b-ad3e-0101c52e7752"));

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Category", "City", "Date", "Description", "Title", "Venue" },
                values: new object[,]
                {
                    { new Guid("7f18c519-b9ac-4719-a098-717e86b629e1"), "Category 1", "City 1", new DateTime(2023, 10, 11, 19, 13, 41, 394, DateTimeKind.Local).AddTicks(1896), "Description 1", "Activity 1", "Venue 1" },
                    { new Guid("ab615f91-113c-412b-a259-b2aa3a126a2b"), "Category 2", "City 2", new DateTime(2023, 10, 11, 19, 13, 41, 394, DateTimeKind.Local).AddTicks(1942), "Description 2", "Activity 2", "Venue 2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("7f18c519-b9ac-4719-a098-717e86b629e1"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("ab615f91-113c-412b-a259-b2aa3a126a2b"));

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Category", "City", "Date", "Description", "Title", "Venue" },
                values: new object[,]
                {
                    { new Guid("a72807cb-1639-4aa1-816d-ed0d8cb0d87a"), "Category 2", "City 2", new DateTime(2023, 10, 8, 12, 54, 37, 606, DateTimeKind.Local).AddTicks(4501), "Description 2", "Activity 2", "Venue 2" },
                    { new Guid("f02482c1-26c3-471b-ad3e-0101c52e7752"), "Category 1", "City 1", new DateTime(2023, 10, 8, 12, 54, 37, 606, DateTimeKind.Local).AddTicks(4456), "Description 1", "Activity 1", "Venue 1" }
                });
        }
    }
}

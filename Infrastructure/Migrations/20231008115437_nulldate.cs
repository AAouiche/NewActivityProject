using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class nulldate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("383c9208-3f6b-4516-aa29-0124947f2d1e"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("61d95d34-a14c-4f55-8616-cb66eae03806"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Activities",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Category", "City", "Date", "Description", "Title", "Venue" },
                values: new object[,]
                {
                    { new Guid("a72807cb-1639-4aa1-816d-ed0d8cb0d87a"), "Category 2", "City 2", new DateTime(2023, 10, 8, 12, 54, 37, 606, DateTimeKind.Local).AddTicks(4501), "Description 2", "Activity 2", "Venue 2" },
                    { new Guid("f02482c1-26c3-471b-ad3e-0101c52e7752"), "Category 1", "City 1", new DateTime(2023, 10, 8, 12, 54, 37, 606, DateTimeKind.Local).AddTicks(4456), "Description 1", "Activity 1", "Venue 1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("a72807cb-1639-4aa1-816d-ed0d8cb0d87a"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("f02482c1-26c3-471b-ad3e-0101c52e7752"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Activities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Category", "City", "Date", "Description", "Title", "Venue" },
                values: new object[,]
                {
                    { new Guid("383c9208-3f6b-4516-aa29-0124947f2d1e"), "Category 1", "City 1", new DateTime(2023, 10, 1, 21, 26, 11, 155, DateTimeKind.Local).AddTicks(2966), "Description 1", "Activity 1", "Venue 1" },
                    { new Guid("61d95d34-a14c-4f55-8616-cb66eae03806"), "Category 2", "City 2", new DateTime(2023, 10, 1, 21, 26, 11, 155, DateTimeKind.Local).AddTicks(3015), "Description 2", "Activity 2", "Venue 2" }
                });
        }
    }
}

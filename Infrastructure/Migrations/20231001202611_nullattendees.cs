using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class nullattendees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("816ae6e4-ae07-4e1f-b2b4-172a943c3621"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("e3ed0a72-9933-4fa5-941a-86a20ed854f5"));

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Category", "City", "Date", "Description", "Title", "Venue" },
                values: new object[,]
                {
                    { new Guid("383c9208-3f6b-4516-aa29-0124947f2d1e"), "Category 1", "City 1", new DateTime(2023, 10, 1, 21, 26, 11, 155, DateTimeKind.Local).AddTicks(2966), "Description 1", "Activity 1", "Venue 1" },
                    { new Guid("61d95d34-a14c-4f55-8616-cb66eae03806"), "Category 2", "City 2", new DateTime(2023, 10, 1, 21, 26, 11, 155, DateTimeKind.Local).AddTicks(3015), "Description 2", "Activity 2", "Venue 2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("383c9208-3f6b-4516-aa29-0124947f2d1e"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("61d95d34-a14c-4f55-8616-cb66eae03806"));

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Category", "City", "Date", "Description", "Title", "Venue" },
                values: new object[,]
                {
                    { new Guid("816ae6e4-ae07-4e1f-b2b4-172a943c3621"), "Category 2", "City 2", new DateTime(2023, 8, 21, 12, 47, 24, 223, DateTimeKind.Local).AddTicks(1377), "Description 2", "Activity 2", "Venue 2" },
                    { new Guid("e3ed0a72-9933-4fa5-941a-86a20ed854f5"), "Category 1", "City 1", new DateTime(2023, 8, 21, 12, 47, 24, 223, DateTimeKind.Local).AddTicks(1329), "Description 1", "Activity 1", "Venue 1" }
                });
        }
    }
}

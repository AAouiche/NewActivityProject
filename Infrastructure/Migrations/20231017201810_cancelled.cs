using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cancelled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("1ac79c3b-da4d-4cc7-bb12-972671a61244"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("7453d819-1749-4646-92d9-6b8331a11da0"));

            migrationBuilder.AddColumn<bool>(
                name: "cancelled",
                table: "Activities",
                type: "bit",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Category", "City", "Date", "Description", "Title", "Venue", "cancelled" },
                values: new object[,]
                {
                    { new Guid("158b84b8-d99b-470c-aac1-de749b7070a6"), "Category 1", "City 1", new DateTime(2023, 10, 17, 21, 18, 10, 274, DateTimeKind.Local).AddTicks(5896), "Description 1", "Activity 1", "Venue 1", null },
                    { new Guid("74dccae7-c1d8-4961-9abe-23d3af0f8adb"), "Category 2", "City 2", new DateTime(2023, 10, 17, 21, 18, 10, 274, DateTimeKind.Local).AddTicks(5948), "Description 2", "Activity 2", "Venue 2", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("158b84b8-d99b-470c-aac1-de749b7070a6"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("74dccae7-c1d8-4961-9abe-23d3af0f8adb"));

            migrationBuilder.DropColumn(
                name: "cancelled",
                table: "Activities");

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Category", "City", "Date", "Description", "Title", "Venue" },
                values: new object[,]
                {
                    { new Guid("1ac79c3b-da4d-4cc7-bb12-972671a61244"), "Category 1", "City 1", new DateTime(2023, 10, 13, 20, 52, 22, 587, DateTimeKind.Local).AddTicks(4243), "Description 1", "Activity 1", "Venue 1" },
                    { new Guid("7453d819-1749-4646-92d9-6b8331a11da0"), "Category 2", "City 2", new DateTime(2023, 10, 13, 20, 52, 22, 587, DateTimeKind.Local).AddTicks(4292), "Description 2", "Activity 2", "Venue 2" }
                });
        }
    }
}

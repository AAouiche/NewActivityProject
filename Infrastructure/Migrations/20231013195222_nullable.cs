using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("7f18c519-b9ac-4719-a098-717e86b629e1"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("ab615f91-113c-412b-a259-b2aa3a126a2b"));

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Category", "City", "Date", "Description", "Title", "Venue" },
                values: new object[,]
                {
                    { new Guid("1ac79c3b-da4d-4cc7-bb12-972671a61244"), "Category 1", "City 1", new DateTime(2023, 10, 13, 20, 52, 22, 587, DateTimeKind.Local).AddTicks(4243), "Description 1", "Activity 1", "Venue 1" },
                    { new Guid("7453d819-1749-4646-92d9-6b8331a11da0"), "Category 2", "City 2", new DateTime(2023, 10, 13, 20, 52, 22, 587, DateTimeKind.Local).AddTicks(4292), "Description 2", "Activity 2", "Venue 2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("1ac79c3b-da4d-4cc7-bb12-972671a61244"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("7453d819-1749-4646-92d9-6b8331a11da0"));

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Category", "City", "Date", "Description", "Title", "Venue" },
                values: new object[,]
                {
                    { new Guid("7f18c519-b9ac-4719-a098-717e86b629e1"), "Category 1", "City 1", new DateTime(2023, 10, 11, 19, 13, 41, 394, DateTimeKind.Local).AddTicks(1896), "Description 1", "Activity 1", "Venue 1" },
                    { new Guid("ab615f91-113c-412b-a259-b2aa3a126a2b"), "Category 2", "City 2", new DateTime(2023, 10, 11, 19, 13, 41, 394, DateTimeKind.Local).AddTicks(1942), "Description 2", "Activity 2", "Venue 2" }
                });
        }
    }
}

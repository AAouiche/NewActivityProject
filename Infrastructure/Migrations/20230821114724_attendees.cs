using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class attendees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("3661fee7-7c85-4338-8dd2-16cd93e458ee"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("989036ec-b5e6-4f5f-991e-e2aa8fc440e3"));

            migrationBuilder.AlterColumn<string>(
                name: "Venue",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Attendees",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsHost = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendees", x => new { x.ApplicationUserId, x.ActivityId });
                    table.ForeignKey(
                        name: "FK_Attendees_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendees_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Category", "City", "Date", "Description", "Title", "Venue" },
                values: new object[,]
                {
                    { new Guid("816ae6e4-ae07-4e1f-b2b4-172a943c3621"), "Category 2", "City 2", new DateTime(2023, 8, 21, 12, 47, 24, 223, DateTimeKind.Local).AddTicks(1377), "Description 2", "Activity 2", "Venue 2" },
                    { new Guid("e3ed0a72-9933-4fa5-941a-86a20ed854f5"), "Category 1", "City 1", new DateTime(2023, 8, 21, 12, 47, 24, 223, DateTimeKind.Local).AddTicks(1329), "Description 1", "Activity 1", "Venue 1" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_ActivityId",
                table: "Attendees",
                column: "ActivityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendees");

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("816ae6e4-ae07-4e1f-b2b4-172a943c3621"));

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: new Guid("e3ed0a72-9933-4fa5-941a-86a20ed854f5"));

            migrationBuilder.AlterColumn<string>(
                name: "Venue",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Activities",
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
                    { new Guid("3661fee7-7c85-4338-8dd2-16cd93e458ee"), "Category 2", "City 2", new DateTime(2023, 8, 13, 1, 23, 31, 928, DateTimeKind.Local).AddTicks(6321), "Description 2", "Activity 2", "Venue 2" },
                    { new Guid("989036ec-b5e6-4f5f-991e-e2aa8fc440e3"), "Category 1", "City 1", new DateTime(2023, 8, 13, 1, 23, 31, 928, DateTimeKind.Local).AddTicks(6278), "Description 1", "Activity 1", "Venue 1" }
                });
        }
    }
}

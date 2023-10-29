using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class imagechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_ApplicationUserId",
                table: "Images");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ApplicationUserId",
                table: "Images",
                column: "ApplicationUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_ApplicationUserId",
                table: "Images");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ApplicationUserId",
                table: "Images",
                column: "ApplicationUserId");
        }
    }
}

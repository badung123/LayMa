using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayMa.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIndexAgentIsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppUsers_Agent",
                table: "AppUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Agent",
                table: "AppUsers",
                column: "Agent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppUsers_Agent",
                table: "AppUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Agent",
                table: "AppUsers",
                column: "Agent",
                unique: true,
                filter: "[Agent] IS NOT NULL");
        }
    }
}

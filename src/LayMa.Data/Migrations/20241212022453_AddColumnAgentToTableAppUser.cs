using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayMa.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnAgentToTableAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Agent",
                table: "AppUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Agent",
                table: "AppUsers",
                column: "Agent",
                unique: true,
                filter: "[Agent] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppUsers_Agent",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Agent",
                table: "AppUsers");
        }
    }
}

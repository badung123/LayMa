using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayMa.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeFiledToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxClickInDay",
                table: "AppUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "AppUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxClickInDay",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "AppUsers");
        }
    }
}

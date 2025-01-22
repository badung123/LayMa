using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayMa.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeRunToCampainTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToTalView",
                table: "Campains");

            migrationBuilder.AddColumn<int>(
                name: "TypeRun",
                table: "Campains",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewPerHour",
                table: "Campains",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeRun",
                table: "Campains");

            migrationBuilder.DropColumn(
                name: "ViewPerHour",
                table: "Campains");

            migrationBuilder.AddColumn<long>(
                name: "ToTalView",
                table: "Campains",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

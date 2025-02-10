using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayMa.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSolutionField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Solution",
                table: "ViewDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Solution",
                table: "TransactionLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Solution",
                table: "Code",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Solution",
                table: "ViewDetail");

            migrationBuilder.DropColumn(
                name: "Solution",
                table: "TransactionLog");

            migrationBuilder.DropColumn(
                name: "Solution",
                table: "Code");
        }
    }
}

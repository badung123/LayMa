using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayMa.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFlatformToTableTranSacyionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Flatform",
                table: "TransactionLog",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flatform",
                table: "TransactionLog");
        }
    }
}

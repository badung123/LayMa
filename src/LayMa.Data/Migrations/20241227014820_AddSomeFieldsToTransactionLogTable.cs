using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayMa.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeFieldsToTransactionLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceScreen",
                table: "TransactionLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "TransactionLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortLink",
                table: "TransactionLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "TransactionLog",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceScreen",
                table: "TransactionLog");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "TransactionLog");

            migrationBuilder.DropColumn(
                name: "ShortLink",
                table: "TransactionLog");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "TransactionLog");
        }
    }
}

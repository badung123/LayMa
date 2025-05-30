using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayMa.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeFinishFieldTOTransactionLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeFinish",
                table: "TransactionLog",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeFinish",
                table: "TransactionLog");
        }
    }
}

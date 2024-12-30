using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayMa.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSDuPhongFieldToTableShortLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Duphong",
                table: "ShortLink",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duphong",
                table: "ShortLink");
        }
    }
}

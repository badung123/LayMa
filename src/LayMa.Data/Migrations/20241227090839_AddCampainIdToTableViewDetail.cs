using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayMa.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCampainIdToTableViewDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CampainId",
                table: "ViewDetail",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CampainId",
                table: "ViewDetail");
        }
    }
}

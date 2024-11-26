using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayMa.Data.Migrations
{
    /// <inheritdoc />
    public partial class addTableCampainAndMission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CampainId",
                table: "Code",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Campains",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KeySearch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyToken = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewPerDay = table.Column<int>(type: "int", nullable: false),
                    PricePerView = table.Column<int>(type: "int", nullable: false),
                    TimeOnSitePerView = table.Column<int>(type: "int", nullable: false),
                    ToTalView = table.Column<long>(type: "bigint", nullable: false),
                    ToTalPrice = table.Column<long>(type: "bigint", nullable: false),
                    Decription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flatform = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemainView = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Missions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShortLinkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenUrl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShortLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Campains");

            migrationBuilder.DropTable(
                name: "Missions");

            migrationBuilder.DropColumn(
                name: "CampainId",
                table: "Code");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourAgency.Migrations
{
    /// <inheritdoc />
    public partial class tourcount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VisitorCount",
                table: "CountryStatistics",
                newName: "TourCount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TourCount",
                table: "CountryStatistics",
                newName: "VisitorCount");
        }
    }
}

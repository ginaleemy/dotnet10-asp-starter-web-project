using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedVilla2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villa",
                columns: new[] { "Id", "CreatedDate", "Details", "ImageUrl", "Name", "Occupancy", "Rate", "Sqft", "UpdatedDate" },
                values: new object[] { 6, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seaview villa surrounded by sea and nature trails.", "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa5.jpg", "Seaview Villa", 3, 975.0, 1500, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villa",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}

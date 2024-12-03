using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVillaApI2.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Amenity",
                table: "Villas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Villas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Villas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Occupancy",
                table: "Villas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Rate",
                table: "Villas",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Sqft",
                table: "Villas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Villas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amenity",
                table: "Villas");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "Villas");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Villas");

            migrationBuilder.DropColumn(
                name: "Occupancy",
                table: "Villas");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Villas");

            migrationBuilder.DropColumn(
                name: "Sqft",
                table: "Villas");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Villas");
        }
    }
}

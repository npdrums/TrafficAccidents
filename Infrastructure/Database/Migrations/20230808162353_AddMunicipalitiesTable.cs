using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddMunicipalitiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "municipalities",
                columns: table => new
                {
                    municipality_id = table.Column<Guid>(type: "uuid", nullable: false),
                    municipality_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    municipality_border = table.Column<Geometry>(type: "geometry(geometry, 4326)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_municipalities", x => x.municipality_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "municipalities");
        }
    }
}

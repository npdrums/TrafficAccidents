using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCitiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "city_id",
                table: "traffic_accidents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    city_id = table.Column<Guid>(type: "uuid", nullable: false),
                    city_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    city_area = table.Column<Geometry>(type: "geometry(geometry, 4326)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cities", x => x.city_id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_traffic_accidents_city_id",
                table: "traffic_accidents",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_cities_city_area",
                table: "cities",
                column: "city_area")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.AddForeignKey(
                name: "fk_traffic_accidents_cities_city_id",
                table: "traffic_accidents",
                column: "city_id",
                principalTable: "cities",
                principalColumn: "city_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_traffic_accidents_cities_city_id",
                table: "traffic_accidents");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropIndex(
                name: "ix_traffic_accidents_city_id",
                table: "traffic_accidents");

            migrationBuilder.DropColumn(
                name: "city_id",
                table: "traffic_accidents");
        }
    }
}

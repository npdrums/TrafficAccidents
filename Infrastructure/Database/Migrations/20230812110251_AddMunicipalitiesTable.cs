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
            migrationBuilder.AddColumn<Guid>(
                name: "municipality_id",
                table: "traffic_accidents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "municipalities",
                columns: table => new
                {
                    municipality_id = table.Column<Guid>(type: "uuid", nullable: false),
                    municipality_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    municipality_area = table.Column<Geometry>(type: "geometry(geometry, 4326)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_municipalities", x => x.municipality_id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_traffic_accidents_accident_location",
                table: "traffic_accidents",
                column: "accident_location")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.CreateIndex(
                name: "ix_traffic_accidents_accident_type",
                table: "traffic_accidents",
                column: "accident_type");

            migrationBuilder.CreateIndex(
                name: "ix_traffic_accidents_municipality_id",
                table: "traffic_accidents",
                column: "municipality_id");

            migrationBuilder.CreateIndex(
                name: "ix_traffic_accidents_participants_nominal_count",
                table: "traffic_accidents",
                column: "participants_nominal_count");

            migrationBuilder.CreateIndex(
                name: "ix_traffic_accidents_participants_status",
                table: "traffic_accidents",
                column: "participants_status");

            migrationBuilder.CreateIndex(
                name: "ix_municipalities_municipality_area",
                table: "municipalities",
                column: "municipality_area")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.AddForeignKey(
                name: "fk_traffic_accidents_municipalities_municipality_id",
                table: "traffic_accidents",
                column: "municipality_id",
                principalTable: "municipalities",
                principalColumn: "municipality_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_traffic_accidents_municipalities_municipality_id",
                table: "traffic_accidents");

            migrationBuilder.DropTable(
                name: "municipalities");

            migrationBuilder.DropIndex(
                name: "ix_traffic_accidents_accident_location",
                table: "traffic_accidents");

            migrationBuilder.DropIndex(
                name: "ix_traffic_accidents_accident_type",
                table: "traffic_accidents");

            migrationBuilder.DropIndex(
                name: "ix_traffic_accidents_municipality_id",
                table: "traffic_accidents");

            migrationBuilder.DropIndex(
                name: "ix_traffic_accidents_participants_nominal_count",
                table: "traffic_accidents");

            migrationBuilder.DropIndex(
                name: "ix_traffic_accidents_participants_status",
                table: "traffic_accidents");

            migrationBuilder.DropColumn(
                name: "municipality_id",
                table: "traffic_accidents");
        }
    }
}

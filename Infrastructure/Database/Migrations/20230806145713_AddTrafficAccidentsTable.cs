using Microsoft.EntityFrameworkCore.Migrations;

using NetTopologySuite.Geometries;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class AddTrafficAccidentsTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("Npgsql:PostgresExtension:postgis", ",,");

        migrationBuilder.CreateTable(
            name: "traffic_accidents",
            columns: table => new
            {
                traffic_accident_id = table.Column<Guid>(type: "uuid", nullable: false),
                external_traffic_accident_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                police_department = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                reported_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                accident_location = table.Column<Point>(type: "geometry (point, 4326)", nullable: false),
                participants_status = table.Column<int>(type: "integer", nullable: false),
                participants_nominal_count = table.Column<int>(type: "integer", nullable: false),
                accident_type = table.Column<int>(type: "integer", nullable: false),
                description = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_traffic_accidents", x => x.traffic_accident_id);
            });

        migrationBuilder.CreateIndex(
            name: "ix_traffic_accidents_police_department",
            table: "traffic_accidents",
            column: "police_department");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "traffic_accidents");
    }
}
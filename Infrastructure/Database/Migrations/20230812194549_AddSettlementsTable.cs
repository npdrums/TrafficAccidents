using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSettlementsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "settlement_id",
                table: "traffic_accidents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "settlements",
                columns: table => new
                {
                    settlement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    settlement_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    settlement_area = table.Column<Geometry>(type: "geometry(geometry, 4326)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_settlements", x => x.settlement_id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_traffic_accidents_settlement_id",
                table: "traffic_accidents",
                column: "settlement_id");

            migrationBuilder.CreateIndex(
                name: "ix_settlements_settlement_area",
                table: "settlements",
                column: "settlement_area")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.AddForeignKey(
                name: "fk_traffic_accidents_settlements_settlement_id",
                table: "traffic_accidents",
                column: "settlement_id",
                principalTable: "settlements",
                principalColumn: "settlement_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_traffic_accidents_settlements_settlement_id",
                table: "traffic_accidents");

            migrationBuilder.DropTable(
                name: "settlements");

            migrationBuilder.DropIndex(
                name: "ix_traffic_accidents_settlement_id",
                table: "traffic_accidents");

            migrationBuilder.DropColumn(
                name: "settlement_id",
                table: "traffic_accidents");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalTrafficAccidentIdIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_traffic_accidents_external_traffic_accident_id",
                table: "traffic_accidents",
                column: "external_traffic_accident_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_traffic_accidents_external_traffic_accident_id",
                table: "traffic_accidents");
        }
    }
}

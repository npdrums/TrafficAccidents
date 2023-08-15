using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseNumberAndIsDeletedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "external_traffic_accident_id",
                table: "traffic_accidents",
                newName: "case_number");

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "traffic_accidents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_traffic_accidents_case_number",
                table: "traffic_accidents",
                column: "case_number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_traffic_accidents_case_number",
                table: "traffic_accidents");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "traffic_accidents");

            migrationBuilder.RenameColumn(
                name: "case_number",
                table: "traffic_accidents",
                newName: "external_traffic_accident_id");
        }
    }
}

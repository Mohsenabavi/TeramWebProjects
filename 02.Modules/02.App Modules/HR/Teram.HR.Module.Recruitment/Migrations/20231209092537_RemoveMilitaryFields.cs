using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMilitaryFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MilitaryServiceCityId",
                schema: "HR",
                table: "BaseInformation");

            migrationBuilder.DropColumn(
                name: "UnitOfmilitaryService",
                schema: "HR",
                table: "BaseInformation");

            migrationBuilder.AddColumn<string>(
                name: "MedicalExemptionReason",
                schema: "HR",
                table: "BaseInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicalExemptionReason",
                schema: "HR",
                table: "BaseInformation");

            migrationBuilder.AddColumn<int>(
                name: "MilitaryServiceCityId",
                schema: "HR",
                table: "BaseInformation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitOfmilitaryService",
                schema: "HR",
                table: "BaseInformation",
                type: "int",
                nullable: true);
        }
    }
}

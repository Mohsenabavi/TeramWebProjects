using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableOfFinalProductInspectionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalProductInspectionId",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.AddColumn<int>(
                name: "FinalProductInspectionId",
                schema: "QCFP",
                table: "FinalProductNoncomplianceDetails",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalProductInspectionId",
                schema: "QCFP",
                table: "FinalProductNoncomplianceDetails");

            migrationBuilder.AddColumn<int>(
                name: "FinalProductInspectionId",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "int",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddWrongDoerInspector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FailureToIdentifyId2",
                schema: "QCFP",
                table: "Causations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FailureToIdentifyId3",
                schema: "QCFP",
                table: "Causations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WrongDoerInspectorId",
                schema: "QCFP",
                table: "Causations",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailureToIdentifyId2",
                schema: "QCFP",
                table: "Causations");

            migrationBuilder.DropColumn(
                name: "FailureToIdentifyId3",
                schema: "QCFP",
                table: "Causations");

            migrationBuilder.DropColumn(
                name: "WrongDoerInspectorId",
                schema: "QCFP",
                table: "Causations");
        }
    }
}

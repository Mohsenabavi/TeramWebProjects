using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddIsIdentified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FailureToIdentifyId",
                schema: "QCFP",
                table: "Causations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsIdentifiableInProduction",
                schema: "QCFP",
                table: "Causations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailureToIdentifyId",
                schema: "QCFP",
                table: "Causations");

            migrationBuilder.DropColumn(
                name: "IsIdentifiableInProduction",
                schema: "QCFP",
                table: "Causations");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddIsIdentifiableRawMaterialDefcets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HasConcessionarylicenseForRawMaterials",
                schema: "QCFP",
                table: "Causations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsIdentifiableRawMaterialDefcets",
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
                name: "HasConcessionarylicenseForRawMaterials",
                schema: "QCFP",
                table: "Causations");

            migrationBuilder.DropColumn(
                name: "IsIdentifiableRawMaterialDefcets",
                schema: "QCFP",
                table: "Causations");
        }
    }
}

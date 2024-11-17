using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddRawMaterialDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RawMaterialDescription",
                schema: "QCFP",
                table: "Causations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RawMaterialDescription",
                schema: "QCFP",
                table: "Causations");
        }
    }
}

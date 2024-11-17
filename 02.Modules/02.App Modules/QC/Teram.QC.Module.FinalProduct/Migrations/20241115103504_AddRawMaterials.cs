using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddRawMaterials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RawMaterialId",
                schema: "QCFP",
                table: "Causations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RawMaterials",
                schema: "QCFP",
                columns: table => new
                {
                    RawMaterialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawMaterials", x => x.RawMaterialId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Causations_RawMaterialId",
                schema: "QCFP",
                table: "Causations",
                column: "RawMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Causations_RawMaterials_RawMaterialId",
                schema: "QCFP",
                table: "Causations",
                column: "RawMaterialId",
                principalSchema: "QCFP",
                principalTable: "RawMaterials",
                principalColumn: "RawMaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Causations_RawMaterials_RawMaterialId",
                schema: "QCFP",
                table: "Causations");

            migrationBuilder.DropTable(
                name: "RawMaterials",
                schema: "QCFP");

            migrationBuilder.DropIndex(
                name: "IX_Causations_RawMaterialId",
                schema: "QCFP",
                table: "Causations");

            migrationBuilder.DropColumn(
                name: "RawMaterialId",
                schema: "QCFP",
                table: "Causations");
        }
    }
}

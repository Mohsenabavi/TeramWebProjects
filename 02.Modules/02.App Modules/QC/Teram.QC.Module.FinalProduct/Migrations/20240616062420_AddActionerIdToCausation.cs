using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddActionerIdToCausation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActionerId",
                schema: "QCFP",
                table: "Causations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Causations_ActionerId",
                schema: "QCFP",
                table: "Causations",
                column: "ActionerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Causations_Actioners_ActionerId",
                schema: "QCFP",
                table: "Causations",
                column: "ActionerId",
                principalSchema: "QCFP",
                principalTable: "Actioners",
                principalColumn: "ActionerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Causations_Actioners_ActionerId",
                schema: "QCFP",
                table: "Causations");

            migrationBuilder.DropIndex(
                name: "IX_Causations_ActionerId",
                schema: "QCFP",
                table: "Causations");

            migrationBuilder.DropColumn(
                name: "ActionerId",
                schema: "QCFP",
                table: "Causations");
        }
    }
}

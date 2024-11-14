using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddOperators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OperatorId2",
                schema: "QCFP",
                table: "Causations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OperatorId3",
                schema: "QCFP",
                table: "Causations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OperatorId4",
                schema: "QCFP",
                table: "Causations",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperatorId2",
                schema: "QCFP",
                table: "Causations");

            migrationBuilder.DropColumn(
                name: "OperatorId3",
                schema: "QCFP",
                table: "Causations");

            migrationBuilder.DropColumn(
                name: "OperatorId4",
                schema: "QCFP",
                table: "Causations");
        }
    }
}

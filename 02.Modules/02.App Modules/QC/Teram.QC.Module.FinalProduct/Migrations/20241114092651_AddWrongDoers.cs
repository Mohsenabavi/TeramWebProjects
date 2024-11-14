using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddWrongDoers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WrongdoerId2",
                schema: "QCFP",
                table: "Causations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WrongdoerId3",
                schema: "QCFP",
                table: "Causations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WrongdoerId4",
                schema: "QCFP",
                table: "Causations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WrongdoerId2",
                schema: "QCFP",
                table: "Causations");

            migrationBuilder.DropColumn(
                name: "WrongdoerId3",
                schema: "QCFP",
                table: "Causations");

            migrationBuilder.DropColumn(
                name: "WrongdoerId4",
                schema: "QCFP",
                table: "Causations");
        }
    }
}

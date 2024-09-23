using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.IncomingGoods.Migrations
{
    /// <inheritdoc />
    public partial class MoveHasFunctionalTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasFunctionalTest",
                schema: "QC",
                table: "IncomingGoodsInspectionItems");

            migrationBuilder.AddColumn<bool>(
                name: "HasFunctionalTest",
                schema: "QC",
                table: "IncomingGoodsInspections",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasFunctionalTest",
                schema: "QC",
                table: "IncomingGoodsInspections");

            migrationBuilder.AddColumn<bool>(
                name: "HasFunctionalTest",
                schema: "QC",
                table: "IncomingGoodsInspectionItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

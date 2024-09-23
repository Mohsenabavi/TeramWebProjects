using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.IncomingGoods.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStatusToBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InspectionStatus",
                schema: "QC",
                table: "IncomingGoodsInspectionItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsMatch",
                schema: "QC",
                table: "IncomingGoodsInspectionItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMatch",
                schema: "QC",
                table: "IncomingGoodsInspectionItems");

            migrationBuilder.AddColumn<int>(
                name: "InspectionStatus",
                schema: "QC",
                table: "IncomingGoodsInspectionItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

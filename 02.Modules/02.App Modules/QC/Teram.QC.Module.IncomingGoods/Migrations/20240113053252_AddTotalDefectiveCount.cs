using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.IncomingGoods.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalDefectiveCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalDefectiveCount",
                schema: "QC",
                table: "IncomingGoodsInspections",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDefectiveCount",
                schema: "QC",
                table: "IncomingGoodsInspections");
        }
    }
}

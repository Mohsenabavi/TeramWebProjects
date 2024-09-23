using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.IncomingGoods.Migrations
{
    /// <inheritdoc />
    public partial class AddIsApprovedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FinalApprove",
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
                name: "FinalApprove",
                schema: "QC",
                table: "IncomingGoodsInspections");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.IncomingGoods.Migrations
{
    /// <inheritdoc />
    public partial class AddSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomingGoodsInspectionCartableItem_IncomingGoodsInspections_IncomingGoodsInspectionId",
                table: "IncomingGoodsInspectionCartableItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IncomingGoodsInspectionCartableItem",
                table: "IncomingGoodsInspectionCartableItem");

            migrationBuilder.RenameTable(
                name: "IncomingGoodsInspectionCartableItem",
                newName: "IncomingGoodsInspectionCartableItems",
                newSchema: "QC");

            migrationBuilder.RenameIndex(
                name: "IX_IncomingGoodsInspectionCartableItem_IncomingGoodsInspectionId",
                schema: "QC",
                table: "IncomingGoodsInspectionCartableItems",
                newName: "IX_IncomingGoodsInspectionCartableItems_IncomingGoodsInspectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IncomingGoodsInspectionCartableItems",
                schema: "QC",
                table: "IncomingGoodsInspectionCartableItems",
                column: "IncomingGoodsInspectionCartableItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomingGoodsInspectionCartableItems_IncomingGoodsInspections_IncomingGoodsInspectionId",
                schema: "QC",
                table: "IncomingGoodsInspectionCartableItems",
                column: "IncomingGoodsInspectionId",
                principalSchema: "QC",
                principalTable: "IncomingGoodsInspections",
                principalColumn: "IncomingGoodsInspectionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomingGoodsInspectionCartableItems_IncomingGoodsInspections_IncomingGoodsInspectionId",
                schema: "QC",
                table: "IncomingGoodsInspectionCartableItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IncomingGoodsInspectionCartableItems",
                schema: "QC",
                table: "IncomingGoodsInspectionCartableItems");

            migrationBuilder.RenameTable(
                name: "IncomingGoodsInspectionCartableItems",
                schema: "QC",
                newName: "IncomingGoodsInspectionCartableItem");

            migrationBuilder.RenameIndex(
                name: "IX_IncomingGoodsInspectionCartableItems_IncomingGoodsInspectionId",
                table: "IncomingGoodsInspectionCartableItem",
                newName: "IX_IncomingGoodsInspectionCartableItem_IncomingGoodsInspectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IncomingGoodsInspectionCartableItem",
                table: "IncomingGoodsInspectionCartableItem",
                column: "IncomingGoodsInspectionCartableItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomingGoodsInspectionCartableItem_IncomingGoodsInspections_IncomingGoodsInspectionId",
                table: "IncomingGoodsInspectionCartableItem",
                column: "IncomingGoodsInspectionId",
                principalSchema: "QC",
                principalTable: "IncomingGoodsInspections",
                principalColumn: "IncomingGoodsInspectionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

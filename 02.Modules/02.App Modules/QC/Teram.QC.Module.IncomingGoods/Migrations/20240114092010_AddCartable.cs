using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.IncomingGoods.Migrations
{
    /// <inheritdoc />
    public partial class AddCartable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomingGoodsInspectionCartableItem",
                columns: table => new
                {
                    IncomingGoodsInspectionCartableItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomingGoodsInspectionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingGoodsInspectionCartableItem", x => x.IncomingGoodsInspectionCartableItemId);
                    table.ForeignKey(
                        name: "FK_IncomingGoodsInspectionCartableItem_IncomingGoodsInspections_IncomingGoodsInspectionId",
                        column: x => x.IncomingGoodsInspectionId,
                        principalSchema: "QC",
                        principalTable: "IncomingGoodsInspections",
                        principalColumn: "IncomingGoodsInspectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomingGoodsInspectionCartableItem_IncomingGoodsInspectionId",
                table: "IncomingGoodsInspectionCartableItem",
                column: "IncomingGoodsInspectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomingGoodsInspectionCartableItem");
        }
    }
}

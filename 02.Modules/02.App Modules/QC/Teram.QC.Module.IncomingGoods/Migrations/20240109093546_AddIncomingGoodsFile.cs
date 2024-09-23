using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.IncomingGoods.Migrations
{
    /// <inheritdoc />
    public partial class AddIncomingGoodsFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomingGoodsInspectionFiles",
                schema: "QC",
                columns: table => new
                {
                    IncomingGoodsInspectionFileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomingGoodsInspectionId = table.Column<int>(type: "int", nullable: false),
                    AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingGoodsInspectionFiles", x => x.IncomingGoodsInspectionFileId);
                    table.ForeignKey(
                        name: "FK_IncomingGoodsInspectionFiles_IncomingGoodsInspections_IncomingGoodsInspectionId",
                        column: x => x.IncomingGoodsInspectionId,
                        principalSchema: "QC",
                        principalTable: "IncomingGoodsInspections",
                        principalColumn: "IncomingGoodsInspectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomingGoodsInspectionFiles_IncomingGoodsInspectionId",
                schema: "QC",
                table: "IncomingGoodsInspectionFiles",
                column: "IncomingGoodsInspectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomingGoodsInspectionFiles",
                schema: "QC");
        }
    }
}

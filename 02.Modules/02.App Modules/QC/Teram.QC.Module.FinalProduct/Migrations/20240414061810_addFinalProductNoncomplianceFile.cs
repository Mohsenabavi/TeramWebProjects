using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class addFinalProductNoncomplianceFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinalProductNoncomplianceFiles",
                schema: "QCFP",
                columns: table => new
                {
                    FinalProductNoncomplianceFileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinalProductNoncomplianceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalProductNoncomplianceFiles", x => x.FinalProductNoncomplianceFileId);
                    table.ForeignKey(
                        name: "FK_FinalProductNoncomplianceFiles_FinalProductNoncompliances_FinalProductNoncomplianceId",
                        column: x => x.FinalProductNoncomplianceId,
                        principalSchema: "QCFP",
                        principalTable: "FinalProductNoncompliances",
                        principalColumn: "FinalProductNoncomplianceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinalProductNoncomplianceFiles_FinalProductNoncomplianceId",
                schema: "QCFP",
                table: "FinalProductNoncomplianceFiles",
                column: "FinalProductNoncomplianceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalProductNoncomplianceFiles",
                schema: "QCFP");
        }
    }
}

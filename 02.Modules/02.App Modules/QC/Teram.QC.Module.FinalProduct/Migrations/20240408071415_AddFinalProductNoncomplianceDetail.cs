using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddFinalProductNoncomplianceDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinalProductNoncomplianceDetails",
                schema: "QCFP",
                columns: table => new
                {
                    FinalProductNoncomplianceDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstSample = table.Column<int>(type: "int", nullable: false),
                    SecondSample = table.Column<int>(type: "int", nullable: false),
                    ThirdSample = table.Column<int>(type: "int", nullable: false),
                    ForthSample = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinalProductNoncomplianceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalProductNoncomplianceDetails", x => x.FinalProductNoncomplianceDetailId);
                    table.ForeignKey(
                        name: "FK_FinalProductNoncomplianceDetails_FinalProductNoncompliances_FinalProductNoncomplianceId",
                        column: x => x.FinalProductNoncomplianceId,
                        principalSchema: "QCFP",
                        principalTable: "FinalProductNoncompliances",
                        principalColumn: "FinalProductNoncomplianceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinalProductNoncomplianceDetails_FinalProductNoncomplianceId",
                schema: "QCFP",
                table: "FinalProductNoncomplianceDetails",
                column: "FinalProductNoncomplianceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalProductNoncomplianceDetails",
                schema: "QCFP");
        }
    }
}

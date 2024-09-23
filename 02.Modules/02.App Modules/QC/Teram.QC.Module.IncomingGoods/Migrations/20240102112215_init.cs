using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.IncomingGoods.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "QC");

            migrationBuilder.CreateTable(
                name: "ControlPlanCategories",
                schema: "QC",
                columns: table => new
                {
                    ControlPlanCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlPlanCategories", x => x.ControlPlanCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "IncomingGoodsInspections",
                schema: "QC",
                columns: table => new
                {
                    IncomingGoodsInspectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QualityInspectionNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoodsTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoodsCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsConsignment = table.Column<bool>(type: "bit", nullable: false),
                    IsSampleGoods = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectionFormStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingGoodsInspections", x => x.IncomingGoodsInspectionId);
                });

            migrationBuilder.CreateTable(
                name: "ControlPlans",
                schema: "QC",
                columns: table => new
                {
                    ControlPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControlPlanParameter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcceptanceCriteria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ControlPlanCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlPlans", x => x.ControlPlanId);
                    table.ForeignKey(
                        name: "FK_ControlPlans_ControlPlanCategories_ControlPlanCategoryId",
                        column: x => x.ControlPlanCategoryId,
                        principalSchema: "QC",
                        principalTable: "ControlPlanCategories",
                        principalColumn: "ControlPlanCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncomingGoodsInspectionItems",
                schema: "QC",
                columns: table => new
                {
                    IncomingGoodsInspectionItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomingGoodsInspectionId = table.Column<int>(type: "int", nullable: false),
                    ControlPlanId = table.Column<int>(type: "int", nullable: false),
                    InspectionResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspectionStatus = table.Column<int>(type: "int", nullable: false),
                    InspectionResultRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountOfDefects = table.Column<int>(type: "int", nullable: false),
                    HasFunctionalTest = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingGoodsInspectionItems", x => x.IncomingGoodsInspectionItemId);
                    table.ForeignKey(
                        name: "FK_IncomingGoodsInspectionItems_ControlPlans_ControlPlanId",
                        column: x => x.ControlPlanId,
                        principalSchema: "QC",
                        principalTable: "ControlPlans",
                        principalColumn: "ControlPlanId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomingGoodsInspectionItems_IncomingGoodsInspections_IncomingGoodsInspectionId",
                        column: x => x.IncomingGoodsInspectionId,
                        principalSchema: "QC",
                        principalTable: "IncomingGoodsInspections",
                        principalColumn: "IncomingGoodsInspectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ControlPlans_ControlPlanCategoryId",
                schema: "QC",
                table: "ControlPlans",
                column: "ControlPlanCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingGoodsInspectionItems_ControlPlanId",
                schema: "QC",
                table: "IncomingGoodsInspectionItems",
                column: "ControlPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingGoodsInspectionItems_IncomingGoodsInspectionId",
                schema: "QC",
                table: "IncomingGoodsInspectionItems",
                column: "IncomingGoodsInspectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomingGoodsInspectionItems",
                schema: "QC");

            migrationBuilder.DropTable(
                name: "ControlPlans",
                schema: "QC");

            migrationBuilder.DropTable(
                name: "IncomingGoodsInspections",
                schema: "QC");

            migrationBuilder.DropTable(
                name: "ControlPlanCategories",
                schema: "QC");
        }
    }
}

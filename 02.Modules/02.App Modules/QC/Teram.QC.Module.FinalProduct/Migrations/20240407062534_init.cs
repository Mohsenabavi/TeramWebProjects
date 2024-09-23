using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "QCFP");

            migrationBuilder.CreateTable(
                name: "FinalProductInspections",
                schema: "QCFP",
                columns: table => new
                {
                    FinalProductInspectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    OrderTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ControlPlan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TracingCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartInterval = table.Column<long>(type: "bigint", nullable: false),
                    EndInterval = table.Column<long>(type: "bigint", nullable: false),
                    SampleCount = table.Column<int>(type: "int", nullable: false),
                    TotalCount = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalProductInspections", x => x.FinalProductInspectionId);
                });

            migrationBuilder.CreateTable(
                name: "QCControlPlans",
                schema: "QCFP",
                columns: table => new
                {
                    QCControlPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QCControlPlans", x => x.QCControlPlanId);
                });

            migrationBuilder.CreateTable(
                name: "QCDefects",
                schema: "QCFP",
                columns: table => new
                {
                    QCDefectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QCDefects", x => x.QCDefectId);
                });

            migrationBuilder.CreateTable(
                name: "AcceptancePeriods",
                schema: "QCFP",
                columns: table => new
                {
                    AcceptancePeriodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QCControlPlanId = table.Column<int>(type: "int", nullable: false),
                    StartInterval = table.Column<long>(type: "bigint", nullable: false),
                    EndInterval = table.Column<long>(type: "bigint", nullable: false),
                    SampleCount = table.Column<int>(type: "int", nullable: false),
                    A = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Total = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcceptancePeriods", x => x.AcceptancePeriodId);
                    table.ForeignKey(
                        name: "FK_AcceptancePeriods_QCControlPlans_QCControlPlanId",
                        column: x => x.QCControlPlanId,
                        principalSchema: "QCFP",
                        principalTable: "QCControlPlans",
                        principalColumn: "QCControlPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ControlPlanDefects",
                schema: "QCFP",
                columns: table => new
                {
                    ControlPlanDefectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QCControlPlanId = table.Column<int>(type: "int", nullable: false),
                    QCDefectId = table.Column<int>(type: "int", nullable: false),
                    ControlPlanDefectVal = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlPlanDefects", x => x.ControlPlanDefectId);
                    table.ForeignKey(
                        name: "FK_ControlPlanDefects_QCControlPlans_QCControlPlanId",
                        column: x => x.QCControlPlanId,
                        principalSchema: "QCFP",
                        principalTable: "QCControlPlans",
                        principalColumn: "QCControlPlanId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ControlPlanDefects_QCDefects_QCDefectId",
                        column: x => x.QCDefectId,
                        principalSchema: "QCFP",
                        principalTable: "QCDefects",
                        principalColumn: "QCDefectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinalProductInspectionDefects",
                schema: "QCFP",
                columns: table => new
                {
                    FinalProductInspectionDefectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinalProductInspectionId = table.Column<int>(type: "int", nullable: false),
                    ControlPlanDefectId = table.Column<int>(type: "int", nullable: false),
                    FirstSample = table.Column<int>(type: "int", nullable: false),
                    SecondSample = table.Column<int>(type: "int", nullable: false),
                    ThirdSample = table.Column<int>(type: "int", nullable: false),
                    ForthSample = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalProductInspectionDefects", x => x.FinalProductInspectionDefectId);
                    table.ForeignKey(
                        name: "FK_FinalProductInspectionDefects_ControlPlanDefects_ControlPlanDefectId",
                        column: x => x.ControlPlanDefectId,
                        principalSchema: "QCFP",
                        principalTable: "ControlPlanDefects",
                        principalColumn: "ControlPlanDefectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinalProductInspectionDefects_FinalProductInspections_FinalProductInspectionId",
                        column: x => x.FinalProductInspectionId,
                        principalSchema: "QCFP",
                        principalTable: "FinalProductInspections",
                        principalColumn: "FinalProductInspectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinalProductNoncompliances",
                schema: "QCFP",
                columns: table => new
                {
                    FinalProductNoncomplianceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinalProductNoncomplianceNumber = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    OrderTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ControlPlan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ControlPlanDefectId = table.Column<int>(type: "int", nullable: false),
                    FinalProductInspectionId = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstSample = table.Column<int>(type: "int", nullable: false),
                    SecondSample = table.Column<int>(type: "int", nullable: false),
                    ThirdSample = table.Column<int>(type: "int", nullable: false),
                    ForthSample = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalProductNoncompliances", x => x.FinalProductNoncomplianceId);
                    table.ForeignKey(
                        name: "FK_FinalProductNoncompliances_ControlPlanDefects_ControlPlanDefectId",
                        column: x => x.ControlPlanDefectId,
                        principalSchema: "QCFP",
                        principalTable: "ControlPlanDefects",
                        principalColumn: "ControlPlanDefectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcceptancePeriods_QCControlPlanId",
                schema: "QCFP",
                table: "AcceptancePeriods",
                column: "QCControlPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlPlanDefects_QCControlPlanId",
                schema: "QCFP",
                table: "ControlPlanDefects",
                column: "QCControlPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlPlanDefects_QCDefectId",
                schema: "QCFP",
                table: "ControlPlanDefects",
                column: "QCDefectId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalProductInspectionDefects_ControlPlanDefectId",
                schema: "QCFP",
                table: "FinalProductInspectionDefects",
                column: "ControlPlanDefectId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalProductInspectionDefects_FinalProductInspectionId",
                schema: "QCFP",
                table: "FinalProductInspectionDefects",
                column: "FinalProductInspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalProductNoncompliances_ControlPlanDefectId",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                column: "ControlPlanDefectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcceptancePeriods",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "FinalProductInspectionDefects",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "FinalProductNoncompliances",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "FinalProductInspections",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "ControlPlanDefects",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "QCControlPlans",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "QCDefects",
                schema: "QCFP");
        }
    }
}

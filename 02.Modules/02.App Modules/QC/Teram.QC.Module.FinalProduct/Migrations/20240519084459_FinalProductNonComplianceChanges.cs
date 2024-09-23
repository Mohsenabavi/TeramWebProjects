using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class FinalProductNonComplianceChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstSample",
                schema: "QCFP",
                table: "FinalProductNoncomplianceDetails");

            migrationBuilder.DropColumn(
                name: "ForthSample",
                schema: "QCFP",
                table: "FinalProductNoncomplianceDetails");

            migrationBuilder.DropColumn(
                name: "SecondSample",
                schema: "QCFP",
                table: "FinalProductNoncomplianceDetails");

            migrationBuilder.DropColumn(
                name: "ThirdSample",
                schema: "QCFP",
                table: "FinalProductNoncomplianceDetails");

            migrationBuilder.AddColumn<int>(
                name: "FormStatus",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasFinalResult",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasSeperationOrder",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasWasteOrder",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeperated",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVoided",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastComment",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifyDate",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NeedToAdvisoryOpinion",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NeedToCkeckByOther",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NeedToRefferToCEO",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QualityControlManagerOpinion",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReferralStatus",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FinalProductNonComplianceCartableItems",
                schema: "QCFP",
                columns: table => new
                {
                    FinalProductNonComplianceCartableItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InputDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OutputDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferredBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FinalProductNoncomplianceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalProductNonComplianceCartableItems", x => x.FinalProductNonComplianceCartableItemId);
                    table.ForeignKey(
                        name: "FK_FinalProductNonComplianceCartableItems_FinalProductNoncompliances_FinalProductNoncomplianceId",
                        column: x => x.FinalProductNoncomplianceId,
                        principalSchema: "QCFP",
                        principalTable: "FinalProductNoncompliances",
                        principalColumn: "FinalProductNoncomplianceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinalProductNoncomplianceDetailSamples",
                schema: "QCFP",
                columns: table => new
                {
                    FinalProductNoncomplianceDetailSampleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SampleType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    FinalProductNoncomplianceDetailId = table.Column<int>(type: "int", nullable: false),
                    OpinionTypeQCManager = table.Column<int>(type: "int", nullable: false),
                    OpinionTypeCEO = table.Column<int>(type: "int", nullable: false),
                    OpinionTypeCEOFinal = table.Column<int>(type: "int", nullable: false),
                    SeparatedCount = table.Column<int>(type: "int", nullable: false),
                    WasteCount = table.Column<int>(type: "int", nullable: false),
                    WasteDocumentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalProductNoncomplianceDetailSamples", x => x.FinalProductNoncomplianceDetailSampleId);
                    table.ForeignKey(
                        name: "FK_FinalProductNoncomplianceDetailSamples_FinalProductNoncomplianceDetails_FinalProductNoncomplianceDetailId",
                        column: x => x.FinalProductNoncomplianceDetailId,
                        principalSchema: "QCFP",
                        principalTable: "FinalProductNoncomplianceDetails",
                        principalColumn: "FinalProductNoncomplianceDetailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowInstructions",
                schema: "QCFP",
                columns: table => new
                {
                    FlowInstructionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromStatus = table.Column<int>(type: "int", nullable: false),
                    ToStatus = table.Column<int>(type: "int", nullable: false),
                    CurrentCartableRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NextCartableRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowInstructions", x => x.FlowInstructionId);
                });

            migrationBuilder.CreateTable(
                name: "Instructions",
                schema: "QCFP",
                columns: table => new
                {
                    InstructionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructions", x => x.InstructionId);
                });

            migrationBuilder.CreateTable(
                name: "Machines",
                schema: "QCFP",
                columns: table => new
                {
                    MachineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.MachineId);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                schema: "QCFP",
                columns: table => new
                {
                    OperatorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.OperatorId);
                });

            migrationBuilder.CreateTable(
                name: "RootCauses",
                schema: "QCFP",
                columns: table => new
                {
                    RootCauseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RootCauses", x => x.RootCauseId);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                schema: "QCFP",
                columns: table => new
                {
                    UnitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitId);
                });

            migrationBuilder.CreateTable(
                name: "WorkStations",
                schema: "QCFP",
                columns: table => new
                {
                    WorkStationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkStations", x => x.WorkStationId);
                });

            migrationBuilder.CreateTable(
                name: "FlowInstructionConditions",
                schema: "QCFP",
                columns: table => new
                {
                    FlowInstructionConditionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlowInstructionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowInstructionConditions", x => x.FlowInstructionConditionId);
                    table.ForeignKey(
                        name: "FK_FlowInstructionConditions_FlowInstructions_FlowInstructionId",
                        column: x => x.FlowInstructionId,
                        principalSchema: "QCFP",
                        principalTable: "FlowInstructions",
                        principalColumn: "FlowInstructionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Causations",
                schema: "QCFP",
                columns: table => new
                {
                    CausationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HasHRCause = table.Column<bool>(type: "bit", nullable: true),
                    HasMethodCause = table.Column<bool>(type: "bit", nullable: true),
                    HasRawMaterialCause = table.Column<bool>(type: "bit", nullable: true),
                    HasEssentialCause = table.Column<bool>(type: "bit", nullable: true),
                    HasEquipmentCause = table.Column<bool>(type: "bit", nullable: true),
                    WorkStationId = table.Column<int>(type: "int", nullable: false),
                    MachineId = table.Column<int>(type: "int", nullable: true),
                    OperatorId = table.Column<int>(type: "int", nullable: true),
                    IsLackOfFit = table.Column<bool>(type: "bit", nullable: true),
                    IsCaseError = table.Column<bool>(type: "bit", nullable: true),
                    HasLackOfFitWorkerAndJob = table.Column<bool>(type: "bit", nullable: true),
                    HasLackOfEducation = table.Column<bool>(type: "bit", nullable: true),
                    HasFailureOfDefineJob = table.Column<bool>(type: "bit", nullable: true),
                    RootCauseId = table.Column<int>(type: "int", nullable: true),
                    InstructionId = table.Column<int>(type: "int", nullable: true),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    CanBeIdentifiedAtEntrance = table.Column<bool>(type: "bit", nullable: true),
                    HasEntitlementLicense = table.Column<bool>(type: "bit", nullable: true),
                    HasNotification = table.Column<bool>(type: "bit", nullable: true),
                    NotificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalProductNoncomplianceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Causations", x => x.CausationId);
                    table.ForeignKey(
                        name: "FK_Causations_FinalProductNoncompliances_FinalProductNoncomplianceId",
                        column: x => x.FinalProductNoncomplianceId,
                        principalSchema: "QCFP",
                        principalTable: "FinalProductNoncompliances",
                        principalColumn: "FinalProductNoncomplianceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Causations_Instructions_InstructionId",
                        column: x => x.InstructionId,
                        principalSchema: "QCFP",
                        principalTable: "Instructions",
                        principalColumn: "InstructionId");
                    table.ForeignKey(
                        name: "FK_Causations_Machines_MachineId",
                        column: x => x.MachineId,
                        principalSchema: "QCFP",
                        principalTable: "Machines",
                        principalColumn: "MachineId");
                    table.ForeignKey(
                        name: "FK_Causations_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalSchema: "QCFP",
                        principalTable: "Operators",
                        principalColumn: "OperatorId");
                    table.ForeignKey(
                        name: "FK_Causations_RootCauses_RootCauseId",
                        column: x => x.RootCauseId,
                        principalSchema: "QCFP",
                        principalTable: "RootCauses",
                        principalColumn: "RootCauseId");
                    table.ForeignKey(
                        name: "FK_Causations_Units_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "QCFP",
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_Causations_WorkStations_WorkStationId",
                        column: x => x.WorkStationId,
                        principalSchema: "QCFP",
                        principalTable: "WorkStations",
                        principalColumn: "WorkStationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Causations_FinalProductNoncomplianceId",
                schema: "QCFP",
                table: "Causations",
                column: "FinalProductNoncomplianceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Causations_InstructionId",
                schema: "QCFP",
                table: "Causations",
                column: "InstructionId");

            migrationBuilder.CreateIndex(
                name: "IX_Causations_MachineId",
                schema: "QCFP",
                table: "Causations",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_Causations_OperatorId",
                schema: "QCFP",
                table: "Causations",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Causations_RootCauseId",
                schema: "QCFP",
                table: "Causations",
                column: "RootCauseId");

            migrationBuilder.CreateIndex(
                name: "IX_Causations_UnitId",
                schema: "QCFP",
                table: "Causations",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Causations_WorkStationId",
                schema: "QCFP",
                table: "Causations",
                column: "WorkStationId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalProductNonComplianceCartableItems_FinalProductNoncomplianceId",
                schema: "QCFP",
                table: "FinalProductNonComplianceCartableItems",
                column: "FinalProductNoncomplianceId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalProductNoncomplianceDetailSamples_FinalProductNoncomplianceDetailId",
                schema: "QCFP",
                table: "FinalProductNoncomplianceDetailSamples",
                column: "FinalProductNoncomplianceDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowInstructionConditions_FlowInstructionId",
                schema: "QCFP",
                table: "FlowInstructionConditions",
                column: "FlowInstructionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Causations",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "FinalProductNonComplianceCartableItems",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "FinalProductNoncomplianceDetailSamples",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "FlowInstructionConditions",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "Instructions",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "Machines",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "Operators",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "RootCauses",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "Units",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "WorkStations",
                schema: "QCFP");

            migrationBuilder.DropTable(
                name: "FlowInstructions",
                schema: "QCFP");

            migrationBuilder.DropColumn(
                name: "FormStatus",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.DropColumn(
                name: "HasFinalResult",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.DropColumn(
                name: "HasSeperationOrder",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.DropColumn(
                name: "HasWasteOrder",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.DropColumn(
                name: "IsSeperated",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.DropColumn(
                name: "IsVoided",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.DropColumn(
                name: "LastComment",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.DropColumn(
                name: "LastModifyDate",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.DropColumn(
                name: "NeedToAdvisoryOpinion",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.DropColumn(
                name: "NeedToCkeckByOther",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.DropColumn(
                name: "NeedToRefferToCEO",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.DropColumn(
                name: "QualityControlManagerOpinion",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.DropColumn(
                name: "ReferralStatus",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.AddColumn<int>(
                name: "FirstSample",
                schema: "QCFP",
                table: "FinalProductNoncomplianceDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ForthSample",
                schema: "QCFP",
                table: "FinalProductNoncomplianceDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondSample",
                schema: "QCFP",
                table: "FinalProductNoncomplianceDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThirdSample",
                schema: "QCFP",
                table: "FinalProductNoncomplianceDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

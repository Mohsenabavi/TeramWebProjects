using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddHseApproveHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FileSummmaryAttachmentId",
                schema: "HR",
                table: "JobApplicants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReferralAtachmentId",
                schema: "HR",
                table: "JobApplicants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HSEApproveHistories",
                schema: "HR",
                columns: table => new
                {
                    HSEApproveHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OccupationalMedicineApproveStatus = table.Column<int>(type: "int", nullable: false),
                    OccupationalMedicineDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OccupationalMedicineApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OccupationalMedicineRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferralAtachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    JobApplicantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HSEApproveHistories", x => x.HSEApproveHistoryId);
                    table.ForeignKey(
                        name: "FK_HSEApproveHistories_JobApplicants_JobApplicantId",
                        column: x => x.JobApplicantId,
                        principalSchema: "HR",
                        principalTable: "JobApplicants",
                        principalColumn: "JobApplicantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HSEApproveHistories_JobApplicantId",
                schema: "HR",
                table: "HSEApproveHistories",
                column: "JobApplicantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HSEApproveHistories",
                schema: "HR");

            migrationBuilder.DropColumn(
                name: "FileSummmaryAttachmentId",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "ReferralAtachmentId",
                schema: "HR",
                table: "JobApplicants");
        }
    }
}

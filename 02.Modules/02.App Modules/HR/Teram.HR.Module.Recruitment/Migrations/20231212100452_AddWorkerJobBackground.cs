using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkerJobBackground : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkerJobBackgrounds",
                schema: "HR",
                columns: table => new
                {
                    WorkerJobBackgroundId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressIMatch = table.Column<bool>(type: "bit", nullable: false),
                    ResumeIsMatch = table.Column<bool>(type: "bit", nullable: false),
                    StatementOfPreviousWorkplace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstApprovePerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstApprovePersonRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondApprovePerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondtApprovePersonRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdApprovePerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdApprovePersonRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearcherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApproveStatus = table.Column<int>(type: "int", nullable: false),
                    JobApplicantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerJobBackgrounds", x => x.WorkerJobBackgroundId);
                    table.ForeignKey(
                        name: "FK_WorkerJobBackgrounds_JobApplicants_JobApplicantId",
                        column: x => x.JobApplicantId,
                        principalSchema: "HR",
                        principalTable: "JobApplicants",
                        principalColumn: "JobApplicantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkerJobBackgrounds_JobApplicantId",
                schema: "HR",
                table: "WorkerJobBackgrounds",
                column: "JobApplicantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkerJobBackgrounds",
                schema: "HR");
        }
    }
}

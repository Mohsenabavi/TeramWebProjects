using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeJobBackground : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeJobBackground",
                columns: table => new
                {
                    EmployeeJobBackgroundId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResumeIsMatch = table.Column<bool>(type: "bit", nullable: false),
                    PerformanceIsApproved = table.Column<bool>(type: "bit", nullable: false),
                    DisciplineIsApproved = table.Column<bool>(type: "bit", nullable: false),
                    MoralityIsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovePerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovePersonRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalStatement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApproveStatus = table.Column<int>(type: "int", nullable: false),
                    JobApplicantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeJobBackground", x => x.EmployeeJobBackgroundId);
                    table.ForeignKey(
                        name: "FK_EmployeeJobBackground_JobApplicants_JobApplicantId",
                        column: x => x.JobApplicantId,
                        principalSchema: "HR",
                        principalTable: "JobApplicants",
                        principalColumn: "JobApplicantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeJobBackground_JobApplicantId",
                table: "EmployeeJobBackground",
                column: "JobApplicantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeJobBackground");
        }
    }
}

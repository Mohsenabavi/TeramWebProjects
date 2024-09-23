using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddJobPostionsAndIntructionLettrs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobPositionId",
                schema: "HR",
                table: "JobApplicants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "JobApplicantsIntroductionLetters",
                schema: "HR",
                columns: table => new
                {
                    JobApplicantsIntroductionLetterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntroductionLetterType = table.Column<int>(type: "int", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobApplicantId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplicantsIntroductionLetters", x => x.JobApplicantsIntroductionLetterId);
                    table.ForeignKey(
                        name: "FK_JobApplicantsIntroductionLetters_JobApplicants_JobApplicantId",
                        column: x => x.JobApplicantId,
                        principalSchema: "HR",
                        principalTable: "JobApplicants",
                        principalColumn: "JobApplicantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobPositions",
                schema: "HR",
                columns: table => new
                {
                    JobPositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPositions", x => x.JobPositionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplicants_JobPositionId",
                schema: "HR",
                table: "JobApplicants",
                column: "JobPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplicantsIntroductionLetters_JobApplicantId",
                schema: "HR",
                table: "JobApplicantsIntroductionLetters",
                column: "JobApplicantId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplicants_JobPositions_JobPositionId",
                schema: "HR",
                table: "JobApplicants",
                column: "JobPositionId",
                principalSchema: "HR",
                principalTable: "JobPositions",
                principalColumn: "JobPositionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplicants_JobPositions_JobPositionId",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropTable(
                name: "JobApplicantsIntroductionLetters",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "JobPositions",
                schema: "HR");

            migrationBuilder.DropIndex(
                name: "IX_JobApplicants_JobPositionId",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "JobPositionId",
                schema: "HR",
                table: "JobApplicants");
        }
    }
}

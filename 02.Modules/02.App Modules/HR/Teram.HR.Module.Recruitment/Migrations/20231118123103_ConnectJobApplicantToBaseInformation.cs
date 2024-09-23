using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class ConnectJobApplicantToBaseInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobApplicantId",
                schema: "HR",
                table: "BaseInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BaseInformation_JobApplicantId",
                schema: "HR",
                table: "BaseInformation",
                column: "JobApplicantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseInformation_JobApplicants_JobApplicantId",
                schema: "HR",
                table: "BaseInformation",
                column: "JobApplicantId",
                principalSchema: "HR",
                principalTable: "JobApplicants",
                principalColumn: "JobApplicantId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseInformation_JobApplicants_JobApplicantId",
                schema: "HR",
                table: "BaseInformation");

            migrationBuilder.DropIndex(
                name: "IX_BaseInformation_JobApplicantId",
                schema: "HR",
                table: "BaseInformation");

            migrationBuilder.DropColumn(
                name: "JobApplicantId",
                schema: "HR",
                table: "BaseInformation");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddFileNameAndExtensionToHSEFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileSummaryFileExtension",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileSummaryFileName",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferralFileExtension",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferralFileName",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileSummaryFileExtension",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "FileSummaryFileName",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "ReferralFileExtension",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "ReferralFileName",
                schema: "HR",
                table: "JobApplicants");
        }
    }
}

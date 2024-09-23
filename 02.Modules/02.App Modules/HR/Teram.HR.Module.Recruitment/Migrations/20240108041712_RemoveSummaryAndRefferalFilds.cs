using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSummaryAndRefferalFilds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "FileSummmaryAttachmentId",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "ReferralAtachmentId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}

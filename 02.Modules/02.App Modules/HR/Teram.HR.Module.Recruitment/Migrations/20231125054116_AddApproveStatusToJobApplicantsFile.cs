using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddApproveStatusToJobApplicantsFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDateTime",
                schema: "HR",
                table: "JobApplicantFiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApproveStatus",
                schema: "HR",
                table: "JobApplicantFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedBy",
                schema: "HR",
                table: "JobApplicantFiles",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveDateTime",
                schema: "HR",
                table: "JobApplicantFiles");

            migrationBuilder.DropColumn(
                name: "ApproveStatus",
                schema: "HR",
                table: "JobApplicantFiles");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                schema: "HR",
                table: "JobApplicantFiles");
        }
    }
}

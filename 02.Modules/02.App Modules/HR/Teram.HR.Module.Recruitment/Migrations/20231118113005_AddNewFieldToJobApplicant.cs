using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldToJobApplicant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveBaseInformation",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.AddColumn<DateTime>(
                name: "BaseInformationApproveDate",
                schema: "HR",
                table: "JobApplicants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BaseInformationApproveStatus",
                schema: "HR",
                table: "JobApplicants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "BaseInformationApprovedBy",
                schema: "HR",
                table: "JobApplicants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseInformationErrors",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                schema: "HR",
                table: "JobApplicants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "HR",
                table: "JobApplicants",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "NationalCode",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProcessStatus",
                schema: "HR",
                table: "JobApplicants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplicants_NationalCode",
                schema: "HR",
                table: "JobApplicants",
                column: "NationalCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobApplicants_NationalCode",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "BaseInformationApproveDate",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "BaseInformationApproveStatus",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "BaseInformationApprovedBy",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "BaseInformationErrors",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "NationalCode",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "ProcessStatus",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.AddColumn<bool>(
                name: "ApproveBaseInformation",
                schema: "HR",
                table: "JobApplicants",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddHseAndNoAdddictionApproves : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NoAddictionApprovedBy",
                schema: "HR",
                table: "JobApplicants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NoAddictionDate",
                schema: "HR",
                table: "JobApplicants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NoAddictionDone",
                schema: "HR",
                table: "JobApplicants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "NoBadBackgroundApprovedBy",
                schema: "HR",
                table: "JobApplicants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NoBadBackgroundDate",
                schema: "HR",
                table: "JobApplicants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NoBadBackgroundDone",
                schema: "HR",
                table: "JobApplicants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OccupationalMedicineApproveStatus",
                schema: "HR",
                table: "JobApplicants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "OccupationalMedicineApprovedBy",
                schema: "HR",
                table: "JobApplicants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OccupationalMedicineDate",
                schema: "HR",
                table: "JobApplicants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OccupationalMedicineRemarks",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoAddictionApprovedBy",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "NoAddictionDate",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "NoAddictionDone",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "NoBadBackgroundApprovedBy",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "NoBadBackgroundDate",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "NoBadBackgroundDone",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "OccupationalMedicineApproveStatus",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "OccupationalMedicineApprovedBy",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "OccupationalMedicineDate",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "OccupationalMedicineRemarks",
                schema: "HR",
                table: "JobApplicants");
        }
    }
}

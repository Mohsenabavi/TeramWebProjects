using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddCreateDateToLetters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "JobApplicantsIntroductionLetters");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                schema: "HR",
                table: "JobApplicantsIntroductionLetters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                schema: "HR",
                table: "JobApplicantsIntroductionLetters");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "JobApplicantsIntroductionLetters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

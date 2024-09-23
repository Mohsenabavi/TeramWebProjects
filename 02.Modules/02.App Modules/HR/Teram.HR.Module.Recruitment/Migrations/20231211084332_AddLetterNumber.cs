using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddLetterNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LetterNumber",
                schema: "HR",
                table: "JobApplicantsIntroductionLetters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LetterNumber",
                schema: "HR",
                table: "JobApplicantsIntroductionLetters");
        }
    }
}

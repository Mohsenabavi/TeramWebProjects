using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class CorrectAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_AnswerDate",
                schema: "HR",
                table: "PersonQuestionnaireQuestions",
                newName: "AnswerDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnswerDate",
                schema: "HR",
                table: "PersonQuestionnaireQuestions",
                newName: "_AnswerDate");
        }
    }
}

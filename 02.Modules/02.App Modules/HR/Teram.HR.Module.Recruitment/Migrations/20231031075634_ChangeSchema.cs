using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonnelQuestionnaire_Questionnaires_QuestionnaireId",
                table: "PersonnelQuestionnaire");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonQuestionnaireQuestions_PersonnelQuestionnaire_PersonnelQuestionnaireId",
                schema: "HR",
                table: "PersonQuestionnaireQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonnelQuestionnaire",
                table: "PersonnelQuestionnaire");

            migrationBuilder.RenameTable(
                name: "PersonnelQuestionnaire",
                newName: "PersonnelQuestionnaires",
                newSchema: "HR");

            migrationBuilder.RenameIndex(
                name: "IX_PersonnelQuestionnaire_QuestionnaireId",
                schema: "HR",
                table: "PersonnelQuestionnaires",
                newName: "IX_PersonnelQuestionnaires_QuestionnaireId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonnelQuestionnaires",
                schema: "HR",
                table: "PersonnelQuestionnaires",
                column: "PersonnelQuestionnaireId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonnelQuestionnaires_Questionnaires_QuestionnaireId",
                schema: "HR",
                table: "PersonnelQuestionnaires",
                column: "QuestionnaireId",
                principalSchema: "HR",
                principalTable: "Questionnaires",
                principalColumn: "QuestionnaireId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonQuestionnaireQuestions_PersonnelQuestionnaires_PersonnelQuestionnaireId",
                schema: "HR",
                table: "PersonQuestionnaireQuestions",
                column: "PersonnelQuestionnaireId",
                principalSchema: "HR",
                principalTable: "PersonnelQuestionnaires",
                principalColumn: "PersonnelQuestionnaireId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonnelQuestionnaires_Questionnaires_QuestionnaireId",
                schema: "HR",
                table: "PersonnelQuestionnaires");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonQuestionnaireQuestions_PersonnelQuestionnaires_PersonnelQuestionnaireId",
                schema: "HR",
                table: "PersonQuestionnaireQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonnelQuestionnaires",
                schema: "HR",
                table: "PersonnelQuestionnaires");

            migrationBuilder.RenameTable(
                name: "PersonnelQuestionnaires",
                schema: "HR",
                newName: "PersonnelQuestionnaire");

            migrationBuilder.RenameIndex(
                name: "IX_PersonnelQuestionnaires_QuestionnaireId",
                table: "PersonnelQuestionnaire",
                newName: "IX_PersonnelQuestionnaire_QuestionnaireId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonnelQuestionnaire",
                table: "PersonnelQuestionnaire",
                column: "PersonnelQuestionnaireId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonnelQuestionnaire_Questionnaires_QuestionnaireId",
                table: "PersonnelQuestionnaire",
                column: "QuestionnaireId",
                principalSchema: "HR",
                principalTable: "Questionnaires",
                principalColumn: "QuestionnaireId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonQuestionnaireQuestions_PersonnelQuestionnaire_PersonnelQuestionnaireId",
                schema: "HR",
                table: "PersonQuestionnaireQuestions",
                column: "PersonnelQuestionnaireId",
                principalTable: "PersonnelQuestionnaire",
                principalColumn: "PersonnelQuestionnaireId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

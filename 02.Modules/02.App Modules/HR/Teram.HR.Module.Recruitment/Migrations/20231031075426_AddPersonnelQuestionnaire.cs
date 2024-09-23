using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonnelQuestionnaire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalCode",
                schema: "HR",
                table: "PersonQuestionnaireQuestions");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "HR",
                table: "PersonQuestionnaireQuestions");

            migrationBuilder.AddColumn<int>(
                name: "PersonnelQuestionnaireId",
                schema: "HR",
                table: "PersonQuestionnaireQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PersonnelQuestionnaire",
                columns: table => new
                {
                    PersonnelQuestionnaireId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionnaireId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonnelQuestionnaire", x => x.PersonnelQuestionnaireId);
                    table.ForeignKey(
                        name: "FK_PersonnelQuestionnaire_Questionnaires_QuestionnaireId",
                        column: x => x.QuestionnaireId,
                        principalSchema: "HR",
                        principalTable: "Questionnaires",
                        principalColumn: "QuestionnaireId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonQuestionnaireQuestions_PersonnelQuestionnaireId",
                schema: "HR",
                table: "PersonQuestionnaireQuestions",
                column: "PersonnelQuestionnaireId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelQuestionnaire_QuestionnaireId",
                table: "PersonnelQuestionnaire",
                column: "QuestionnaireId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonQuestionnaireQuestions_PersonnelQuestionnaire_PersonnelQuestionnaireId",
                schema: "HR",
                table: "PersonQuestionnaireQuestions",
                column: "PersonnelQuestionnaireId",
                principalTable: "PersonnelQuestionnaire",
                principalColumn: "PersonnelQuestionnaireId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonQuestionnaireQuestions_PersonnelQuestionnaire_PersonnelQuestionnaireId",
                schema: "HR",
                table: "PersonQuestionnaireQuestions");

            migrationBuilder.DropTable(
                name: "PersonnelQuestionnaire");

            migrationBuilder.DropIndex(
                name: "IX_PersonQuestionnaireQuestions_PersonnelQuestionnaireId",
                schema: "HR",
                table: "PersonQuestionnaireQuestions");

            migrationBuilder.DropColumn(
                name: "PersonnelQuestionnaireId",
                schema: "HR",
                table: "PersonQuestionnaireQuestions");

            migrationBuilder.AddColumn<string>(
                name: "NationalCode",
                schema: "HR",
                table: "PersonQuestionnaireQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "HR",
                table: "PersonQuestionnaireQuestions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}

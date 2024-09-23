using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonQuestionaire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonQuestionnaireQuestions",
                schema: "HR",
                columns: table => new
                {
                    PersonQuestionnaireQuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionnaireQuestionId = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<int>(type: "int", nullable: false),
                    _AnswerDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonQuestionnaireQuestions", x => x.PersonQuestionnaireQuestionId);
                    table.ForeignKey(
                        name: "FK_PersonQuestionnaireQuestions_QuestionnaireQuestions_QuestionnaireQuestionId",
                        column: x => x.QuestionnaireQuestionId,
                        principalSchema: "HR",
                        principalTable: "QuestionnaireQuestions",
                        principalColumn: "QuestionnaireQuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonQuestionnaireQuestions_QuestionnaireQuestionId",
                schema: "HR",
                table: "PersonQuestionnaireQuestions",
                column: "QuestionnaireQuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonQuestionnaireQuestions",
                schema: "HR");
        }
    }
}

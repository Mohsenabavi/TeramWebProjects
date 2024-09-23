using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguageAndSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonnelComputerSkills",
                schema: "HR",
                columns: table => new
                {
                    PersonnelComputerSkillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComputerSkill = table.Column<int>(type: "int", nullable: false),
                    SkillLevel = table.Column<int>(type: "int", nullable: false),
                    BaseInformationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonnelComputerSkills", x => x.PersonnelComputerSkillId);
                    table.ForeignKey(
                        name: "FK_PersonnelComputerSkills_BaseInformation_BaseInformationId",
                        column: x => x.BaseInformationId,
                        principalSchema: "HR",
                        principalTable: "BaseInformation",
                        principalColumn: "BaseInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonnelLanguages",
                schema: "HR",
                columns: table => new
                {
                    PersonnelLanguageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<int>(type: "int", nullable: false),
                    SkillLevel = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseInformationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonnelLanguages", x => x.PersonnelLanguageId);
                    table.ForeignKey(
                        name: "FK_PersonnelLanguages_BaseInformation_BaseInformationId",
                        column: x => x.BaseInformationId,
                        principalSchema: "HR",
                        principalTable: "BaseInformation",
                        principalColumn: "BaseInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelComputerSkills_BaseInformationId",
                schema: "HR",
                table: "PersonnelComputerSkills",
                column: "BaseInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelLanguages_BaseInformationId",
                schema: "HR",
                table: "PersonnelLanguages",
                column: "BaseInformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonnelComputerSkills",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "PersonnelLanguages",
                schema: "HR");
        }
    }
}

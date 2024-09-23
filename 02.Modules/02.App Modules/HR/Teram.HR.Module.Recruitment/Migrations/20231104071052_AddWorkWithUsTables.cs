using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkWithUsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseInformation",
                schema: "HR",
                columns: table => new
                {
                    BaseInformationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentityNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentitySerialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthLocationId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UnitOfmilitaryService = table.Column<int>(type: "int", nullable: true),
                    MilitaryServiceCityId = table.Column<int>(type: "int", nullable: true),
                    StartDateMilitaryService = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDateMilitaryService = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MilitaryServiceStatus = table.Column<int>(type: "int", nullable: true),
                    RequiredSalary = table.Column<double>(type: "float", nullable: true),
                    InsuranceMonths = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BloodGroup = table.Column<int>(type: "int", nullable: true),
                    Religion = table.Column<int>(type: "int", nullable: true),
                    Sect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarriageStatus = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsuranceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeCityId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Citizenship = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,15)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,15)", nullable: true),
                    PartnerEducationLevel = table.Column<int>(type: "int", nullable: true),
                    PartnerJob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildCount = table.Column<int>(type: "int", nullable: false),
                    HasWorkingRelatives = table.Column<bool>(type: "bit", nullable: false),
                    WorkingRelatives = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThesisTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialAcademicAchievements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialWorkSuccesses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FreeTimeActivities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasSpecialDisease = table.Column<bool>(type: "bit", nullable: false),
                    SpecialDisease = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasCriminalRecord = table.Column<bool>(type: "bit", nullable: false),
                    CriminalRecord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanWorkInShifts = table.Column<bool>(type: "bit", nullable: false),
                    BusinessMissionAbility = table.Column<bool>(type: "bit", nullable: false),
                    HasIntentionToMigrate = table.Column<bool>(type: "bit", nullable: false),
                    ReadyToWorkDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseInformation", x => x.BaseInformationId);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                schema: "HR",
                columns: table => new
                {
                    EducationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseInformationId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducationDurationHour = table.Column<int>(type: "int", nullable: false),
                    EducationYear = table.Column<int>(type: "int", nullable: false),
                    HasCertificate = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.EducationId);
                    table.ForeignKey(
                        name: "FK_Educations_BaseInformation_BaseInformationId",
                        column: x => x.BaseInformationId,
                        principalSchema: "HR",
                        principalTable: "BaseInformation",
                        principalColumn: "BaseInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyContacts",
                schema: "HR",
                columns: table => new
                {
                    EmergencyContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseInformationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Relation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyContacts", x => x.EmergencyContactId);
                    table.ForeignKey(
                        name: "FK_EmergencyContacts_BaseInformation_BaseInformationId",
                        column: x => x.BaseInformationId,
                        principalSchema: "HR",
                        principalTable: "BaseInformation",
                        principalColumn: "BaseInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resumes",
                schema: "HR",
                columns: table => new
                {
                    ResumeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseInformationId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndCooperationReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecivedSalary = table.Column<double>(type: "float", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationalPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectSupervisorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectSupervisorPhone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resumes", x => x.ResumeId);
                    table.ForeignKey(
                        name: "FK_Resumes_BaseInformation_BaseInformationId",
                        column: x => x.BaseInformationId,
                        principalSchema: "HR",
                        principalTable: "BaseInformation",
                        principalColumn: "BaseInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingRecords",
                schema: "HR",
                columns: table => new
                {
                    TrainingRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseInformationId = table.Column<int>(type: "int", nullable: false),
                    CollegeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducationGrade = table.Column<int>(type: "int", nullable: true),
                    Average = table.Column<double>(type: "float", nullable: true),
                    Field = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducationCityId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingRecords", x => x.TrainingRecordId);
                    table.ForeignKey(
                        name: "FK_TrainingRecords_BaseInformation_BaseInformationId",
                        column: x => x.BaseInformationId,
                        principalSchema: "HR",
                        principalTable: "BaseInformation",
                        principalColumn: "BaseInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Educations_BaseInformationId",
                schema: "HR",
                table: "Educations",
                column: "BaseInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContacts_BaseInformationId",
                schema: "HR",
                table: "EmergencyContacts",
                column: "BaseInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_BaseInformationId",
                schema: "HR",
                table: "Resumes",
                column: "BaseInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingRecords_BaseInformationId",
                schema: "HR",
                table: "TrainingRecords",
                column: "BaseInformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Educations",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "EmergencyContacts",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Resumes",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "TrainingRecords",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "BaseInformation",
                schema: "HR");
        }
    }
}

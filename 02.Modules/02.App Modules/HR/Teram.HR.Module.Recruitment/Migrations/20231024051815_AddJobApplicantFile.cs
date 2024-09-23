using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddJobApplicantFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobApplicantFiles",
                schema: "HR",
                columns: table => new
                {
                    JobApplicantFileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobApplicantId = table.Column<int>(type: "int", nullable: false),
                    AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplicantFiles", x => x.JobApplicantFileId);
                    table.ForeignKey(
                        name: "FK_JobApplicantFiles_JobApplicants_JobApplicantId",
                        column: x => x.JobApplicantId,
                        principalSchema: "HR",
                        principalTable: "JobApplicants",
                        principalColumn: "JobApplicantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplicantFiles_JobApplicantId",
                schema: "HR",
                table: "JobApplicantFiles",
                column: "JobApplicantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobApplicantFiles",
                schema: "HR");
        }
    }
}

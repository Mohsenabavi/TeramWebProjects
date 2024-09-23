using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NationalCode",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BirthCertificateNo",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BirthCertificateSerial",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Citizenship",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Family",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GenderType",
                schema: "HR",
                table: "JobApplicants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                schema: "HR",
                table: "JobApplicants",
                type: "decimal(18,15)",
                precision: 18,
                scale: 15,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                schema: "HR",
                table: "JobApplicants",
                type: "decimal(18,15)",
                precision: 18,
                scale: 15,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MajorId",
                schema: "HR",
                table: "JobApplicants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaritalStatus",
                schema: "HR",
                table: "JobApplicants",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Religion",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Majors",
                schema: "HR",
                columns: table => new
                {
                    MajorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.MajorId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplicants_MajorId",
                schema: "HR",
                table: "JobApplicants",
                column: "MajorId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplicants_Majors_MajorId",
                schema: "HR",
                table: "JobApplicants",
                column: "MajorId",
                principalSchema: "HR",
                principalTable: "Majors",
                principalColumn: "MajorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplicants_Majors_MajorId",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropTable(
                name: "Majors",
                schema: "HR");

            migrationBuilder.DropIndex(
                name: "IX_JobApplicants_MajorId",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "Address",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "BirthCertificateNo",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "BirthCertificateSerial",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "Citizenship",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "Family",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "GenderType",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "MajorId",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "Nationality",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "Religion",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.AlterColumn<int>(
                name: "NationalCode",
                schema: "HR",
                table: "JobApplicants",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}

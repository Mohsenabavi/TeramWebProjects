using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddIdenitiyNoAndFatherNameToJA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                schema: "HR",
                table: "JobApplicants",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FatherName",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                schema: "HR",
                table: "JobApplicants");
        }
    }
}

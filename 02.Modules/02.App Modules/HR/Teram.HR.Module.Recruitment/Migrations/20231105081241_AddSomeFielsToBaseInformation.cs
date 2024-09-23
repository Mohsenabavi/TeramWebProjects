using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeFielsToBaseInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecivedSalary",
                schema: "HR",
                table: "Resumes");

            migrationBuilder.AddColumn<string>(
                name: "CurrentJobActivity",
                schema: "HR",
                table: "BaseInformation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrntSalary",
                schema: "HR",
                table: "BaseInformation",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasWorkingRelativeInPackingCompanies",
                schema: "HR",
                table: "BaseInformation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WorkingRelativeInPackingCompanyName",
                schema: "HR",
                table: "BaseInformation",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentJobActivity",
                schema: "HR",
                table: "BaseInformation");

            migrationBuilder.DropColumn(
                name: "CurrntSalary",
                schema: "HR",
                table: "BaseInformation");

            migrationBuilder.DropColumn(
                name: "HasWorkingRelativeInPackingCompanies",
                schema: "HR",
                table: "BaseInformation");

            migrationBuilder.DropColumn(
                name: "WorkingRelativeInPackingCompanyName",
                schema: "HR",
                table: "BaseInformation");

            migrationBuilder.AddColumn<double>(
                name: "RecivedSalary",
                schema: "HR",
                table: "Resumes",
                type: "float",
                nullable: true);
        }
    }
}

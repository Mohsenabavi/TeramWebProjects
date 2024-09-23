using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class RemoveReligionAndSalary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Religion",
                schema: "HR",
                table: "BaseInformation");

            migrationBuilder.DropColumn(
                name: "RequiredSalary",
                schema: "HR",
                table: "BaseInformation");

            migrationBuilder.DropColumn(
                name: "Sect",
                schema: "HR",
                table: "BaseInformation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Religion",
                schema: "HR",
                table: "BaseInformation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RequiredSalary",
                schema: "HR",
                table: "BaseInformation",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sect",
                schema: "HR",
                table: "BaseInformation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

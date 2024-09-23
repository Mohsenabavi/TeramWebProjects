using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddApproverId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approver",
                schema: "QCFP",
                table: "CorrectiveActions");

            migrationBuilder.AddColumn<int>(
                name: "ApproverId",
                schema: "QCFP",
                table: "CorrectiveActions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproverId",
                schema: "QCFP",
                table: "CorrectiveActions");

            migrationBuilder.AddColumn<string>(
                name: "Approver",
                schema: "QCFP",
                table: "CorrectiveActions",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}

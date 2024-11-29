using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddFailureToFollowInstructions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FailureToFollowInstructions",
                schema: "QCFP",
                table: "Causations",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailureToFollowInstructions",
                schema: "QCFP",
                table: "Causations");
        }
    }
}

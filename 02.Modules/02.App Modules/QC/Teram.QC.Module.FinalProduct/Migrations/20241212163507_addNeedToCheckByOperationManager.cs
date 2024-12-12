using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class addNeedToCheckByOperationManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedToCheckByOperationManager",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedToCheckByOperationManager",
                schema: "QCFP",
                table: "FinalProductNoncompliances");
        }
    }
}

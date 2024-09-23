using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.IT.Module.BackupManagement.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDestinationPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InternalDestinationPath",
                schema: "BK",
                table: "Applications",
                newName: "DestinationPath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DestinationPath",
                schema: "BK",
                table: "Applications",
                newName: "InternalDestinationPath");
        }
    }
}

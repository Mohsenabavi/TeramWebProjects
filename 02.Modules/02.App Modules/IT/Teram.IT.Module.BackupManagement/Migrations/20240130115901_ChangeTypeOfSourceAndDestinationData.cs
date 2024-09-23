using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.IT.Module.BackupManagement.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTypeOfSourceAndDestinationData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServerPathType",
                schema: "BK",
                table: "ServerPaths");

            migrationBuilder.RenameColumn(
                name: "Path",
                schema: "BK",
                table: "ServerPaths",
                newName: "SourcePath");

            migrationBuilder.AddColumn<string>(
                name: "DestinationPath",
                schema: "BK",
                table: "ServerPaths",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationPath",
                schema: "BK",
                table: "ServerPaths");

            migrationBuilder.RenameColumn(
                name: "SourcePath",
                schema: "BK",
                table: "ServerPaths",
                newName: "Path");

            migrationBuilder.AddColumn<int>(
                name: "ServerPathType",
                schema: "BK",
                table: "ServerPaths",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

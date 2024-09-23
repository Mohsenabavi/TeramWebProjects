using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.IT.Module.BackupManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddBackupHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackupHistory",
                schema: "BK",
                columns: table => new
                {
                    BackupHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BackupDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourcePath = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DestinationPath = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackupHistory", x => x.BackupHistoryId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackupHistory",
                schema: "BK");
        }
    }
}

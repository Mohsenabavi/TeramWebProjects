using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.IT.Module.BackupManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddRunFinishDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RunFinishDate",
                schema: "BK",
                table: "JobRunHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RunFinishDate",
                schema: "BK",
                table: "JobRunHistory");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.IT.Module.BackupManagement.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BK");

            migrationBuilder.CreateTable(
                name: "Applications",
                schema: "BK",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "ServerPaths",
                schema: "BK",
                columns: table => new
                {
                    ServerPathId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServerPathType = table.Column<int>(type: "int", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerPaths", x => x.ServerPathId);
                    table.ForeignKey(
                        name: "FK_ServerPaths_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalSchema: "BK",
                        principalTable: "Applications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServerPaths_ApplicationId",
                schema: "BK",
                table: "ServerPaths",
                column: "ApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerPaths",
                schema: "BK");

            migrationBuilder.DropTable(
                name: "Applications",
                schema: "BK");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.OC.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "OC");

            migrationBuilder.CreateTable(
                name: "Position",
                schema: "OC",
                columns: table => new
                {
                    PositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.PositionId);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationChart",
                schema: "OC",
                columns: table => new
                {
                    OrganizationChartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentOrganizationChartId = table.Column<int>(type: "int", nullable: true),
                    PersonnelCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationChart", x => x.OrganizationChartId);
                    table.ForeignKey(
                        name: "FK_OrganizationChart_OrganizationChart_ParentOrganizationChartId",
                        column: x => x.ParentOrganizationChartId,
                        principalSchema: "OC",
                        principalTable: "OrganizationChart",
                        principalColumn: "OrganizationChartId");
                    table.ForeignKey(
                        name: "FK_OrganizationChart_Position_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "OC",
                        principalTable: "Position",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationChart_ParentOrganizationChartId",
                schema: "OC",
                table: "OrganizationChart",
                column: "ParentOrganizationChartId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationChart_PositionId",
                schema: "OC",
                table: "OrganizationChart",
                column: "PositionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationChart",
                schema: "OC");

            migrationBuilder.DropTable(
                name: "Position",
                schema: "OC");
        }
    }
}

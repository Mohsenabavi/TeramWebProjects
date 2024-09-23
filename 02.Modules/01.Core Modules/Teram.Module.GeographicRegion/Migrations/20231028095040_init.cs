using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.Module.GeographicRegion.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Geo");

            migrationBuilder.CreateTable(
                name: "GeographicRegions",
                schema: "Geo",
                columns: table => new
                {
                    GeographicRegionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentGeographicRegionId = table.Column<int>(type: "int", nullable: true),
                    GeographicRegionTypeId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LatinName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeographicRegions", x => x.GeographicRegionId);
                    table.ForeignKey(
                        name: "FK_GeographicRegions_GeographicRegions_ParentGeographicRegionId",
                        column: x => x.ParentGeographicRegionId,
                        principalSchema: "Geo",
                        principalTable: "GeographicRegions",
                        principalColumn: "GeographicRegionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeographicRegions_ParentGeographicRegionId",
                schema: "Geo",
                table: "GeographicRegions",
                column: "ParentGeographicRegionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeographicRegions",
                schema: "Geo");
        }
    }
}

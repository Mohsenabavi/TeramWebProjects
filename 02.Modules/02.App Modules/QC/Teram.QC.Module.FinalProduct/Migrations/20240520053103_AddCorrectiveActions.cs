using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrectiveActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CorrectiveActions",
                schema: "QCFP",
                columns: table => new
                {
                    CorrectiveActionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Actioner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descriiption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CausationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrectiveActions", x => x.CorrectiveActionId);
                    table.ForeignKey(
                        name: "FK_CorrectiveActions_Causations_CausationId",
                        column: x => x.CausationId,
                        principalSchema: "QCFP",
                        principalTable: "Causations",
                        principalColumn: "CausationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorrectiveActions_CausationId",
                schema: "QCFP",
                table: "CorrectiveActions",
                column: "CausationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorrectiveActions",
                schema: "QCFP");
        }
    }
}

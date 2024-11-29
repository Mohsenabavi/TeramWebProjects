using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddMachineryCause : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MachineryCauseId",
                schema: "QCFP",
                table: "Causations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MachineryCause",
                columns: table => new
                {
                    MachineryCauseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineryCause", x => x.MachineryCauseId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Causations_MachineryCauseId",
                schema: "QCFP",
                table: "Causations",
                column: "MachineryCauseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Causations_MachineryCause_MachineryCauseId",
                schema: "QCFP",
                table: "Causations",
                column: "MachineryCauseId",
                principalTable: "MachineryCause",
                principalColumn: "MachineryCauseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Causations_MachineryCause_MachineryCauseId",
                schema: "QCFP",
                table: "Causations");

            migrationBuilder.DropTable(
                name: "MachineryCause");

            migrationBuilder.DropIndex(
                name: "IX_Causations_MachineryCauseId",
                schema: "QCFP",
                table: "Causations");

            migrationBuilder.DropColumn(
                name: "MachineryCauseId",
                schema: "QCFP",
                table: "Causations");
        }
    }
}

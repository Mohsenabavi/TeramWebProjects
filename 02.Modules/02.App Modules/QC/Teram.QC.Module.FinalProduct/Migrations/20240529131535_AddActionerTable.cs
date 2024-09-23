using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddActionerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actioner",
                schema: "QCFP",
                table: "CorrectiveActions");

            migrationBuilder.AddColumn<string>(
                name: "PersonnelCode",
                schema: "QCFP",
                table: "Operators",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActionerId",
                schema: "QCFP",
                table: "CorrectiveActions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Actioners",
                schema: "QCFP",
                columns: table => new
                {
                    ActionerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PersonnelCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PostTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actioners", x => x.ActionerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorrectiveActions_ActionerId",
                schema: "QCFP",
                table: "CorrectiveActions",
                column: "ActionerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CorrectiveActions_Actioners_ActionerId",
                schema: "QCFP",
                table: "CorrectiveActions",
                column: "ActionerId",
                principalSchema: "QCFP",
                principalTable: "Actioners",
                principalColumn: "ActionerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorrectiveActions_Actioners_ActionerId",
                schema: "QCFP",
                table: "CorrectiveActions");

            migrationBuilder.DropTable(
                name: "Actioners",
                schema: "QCFP");

            migrationBuilder.DropIndex(
                name: "IX_CorrectiveActions_ActionerId",
                schema: "QCFP",
                table: "CorrectiveActions");

            migrationBuilder.DropColumn(
                name: "PersonnelCode",
                schema: "QCFP",
                table: "Operators");

            migrationBuilder.DropColumn(
                name: "ActionerId",
                schema: "QCFP",
                table: "CorrectiveActions");

            migrationBuilder.AddColumn<string>(
                name: "Actioner",
                schema: "QCFP",
                table: "CorrectiveActions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

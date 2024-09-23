using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class ActionerNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorrectiveActions_Actioners_ActionerId",
                schema: "QCFP",
                table: "CorrectiveActions");

            migrationBuilder.AlterColumn<int>(
                name: "ActionerId",
                schema: "QCFP",
                table: "CorrectiveActions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CorrectiveActions_Actioners_ActionerId",
                schema: "QCFP",
                table: "CorrectiveActions",
                column: "ActionerId",
                principalSchema: "QCFP",
                principalTable: "Actioners",
                principalColumn: "ActionerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorrectiveActions_Actioners_ActionerId",
                schema: "QCFP",
                table: "CorrectiveActions");

            migrationBuilder.AlterColumn<int>(
                name: "ActionerId",
                schema: "QCFP",
                table: "CorrectiveActions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CorrectiveActions_Actioners_ActionerId",
                schema: "QCFP",
                table: "CorrectiveActions",
                column: "ActionerId",
                principalSchema: "QCFP",
                principalTable: "Actioners",
                principalColumn: "ActionerId");
        }
    }
}

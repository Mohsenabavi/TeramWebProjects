using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.TicketRegister.Migrations
{
    /// <inheritdoc />
    public partial class addRowNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "Ticket",
                table: "Areas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RowNumber",
                schema: "Ticket",
                table: "AreaRows",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                schema: "Ticket",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "RowNumber",
                schema: "Ticket",
                table: "AreaRows");
        }
    }
}

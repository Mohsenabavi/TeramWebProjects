using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.Module.Authentication.Migrations
{
    /// <inheritdoc />
    public partial class AddPassWordChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PassWordChanged",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassWordChanged",
                table: "AspNetUsers");
        }
    }
}

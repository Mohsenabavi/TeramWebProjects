using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.Module.Authentication.Migrations
{
    /// <inheritdoc />
    public partial class AddLoginCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoginCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginCount",
                table: "AspNetUsers");
        }
    }
}

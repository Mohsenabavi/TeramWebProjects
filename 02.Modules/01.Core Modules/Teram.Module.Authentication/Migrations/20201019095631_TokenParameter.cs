using Microsoft.EntityFrameworkCore.Migrations;

namespace Teram.Module.Authentication.Migrations
{
    public partial class TokenParameter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TokenParameters",
                schema: "api",
                columns: table => new
                {
                    TokenParameterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenParameters", x => x.TokenParameterId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenParameters",
                schema: "api");
        }
    }
}

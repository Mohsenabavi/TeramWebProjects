using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Teram.Module.Authentication.Migrations
{
    public partial class TokenInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "api");

            migrationBuilder.CreateTable(
                name: "Tokens",
                schema: "api",
                columns: table => new
                {
                    TokenId = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    IssuerId = table.Column<Guid>(nullable: false),
                    IssuedOn = table.Column<DateTime>(nullable: false),
                    IssuedFor = table.Column<string>(nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Policy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.TokenId);
                    table.ForeignKey(
                        name: "FK_Tokens_AspNetUsers_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_IssuerId",
                schema: "api",
                table: "Tokens",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_UserId",
                schema: "api",
                table: "Tokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tokens",
                schema: "api");
        }
    }
}

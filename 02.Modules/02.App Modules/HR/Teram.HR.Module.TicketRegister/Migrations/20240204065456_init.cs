using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.TicketRegister.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Ticket");

            migrationBuilder.CreateTable(
                name: "Areas",
                schema: "Ticket",
                columns: table => new
                {
                    AreaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.AreaId);
                });

            migrationBuilder.CreateTable(
                name: "AreaRows",
                schema: "Ticket",
                columns: table => new
                {
                    AreaRowId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeatCount = table.Column<int>(type: "int", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaRows", x => x.AreaRowId);
                    table.ForeignKey(
                        name: "FK_AreaRows_Areas_AreaId",
                        column: x => x.AreaId,
                        principalSchema: "Ticket",
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                schema: "Ticket",
                columns: table => new
                {
                    SeatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeatNumber = table.Column<int>(type: "int", nullable: false),
                    IsReserved = table.Column<bool>(type: "bit", nullable: false),
                    ReservedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReservedFor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaRowId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.SeatId);
                    table.ForeignKey(
                        name: "FK_Seats_AreaRows_AreaRowId",
                        column: x => x.AreaRowId,
                        principalSchema: "Ticket",
                        principalTable: "AreaRows",
                        principalColumn: "AreaRowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaRows_AreaId",
                schema: "Ticket",
                table: "AreaRows",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_AreaRowId",
                schema: "Ticket",
                table: "Seats",
                column: "AreaRowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seats",
                schema: "Ticket");

            migrationBuilder.DropTable(
                name: "AreaRows",
                schema: "Ticket");

            migrationBuilder.DropTable(
                name: "Areas",
                schema: "Ticket");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.IncomingGoods.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsCartable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InputDate",
                table: "IncomingGoodsInspectionCartableItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "OutputDate",
                table: "IncomingGoodsInspectionCartableItem",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReferredBy",
                table: "IncomingGoodsInspectionCartableItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputDate",
                table: "IncomingGoodsInspectionCartableItem");

            migrationBuilder.DropColumn(
                name: "OutputDate",
                table: "IncomingGoodsInspectionCartableItem");

            migrationBuilder.DropColumn(
                name: "ReferredBy",
                table: "IncomingGoodsInspectionCartableItem");
        }
    }
}

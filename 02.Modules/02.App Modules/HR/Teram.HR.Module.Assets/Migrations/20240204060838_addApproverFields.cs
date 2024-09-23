using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Assets.Migrations
{
    /// <inheritdoc />
    public partial class addApproverFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDate",
                schema: "Rahkaran",
                table: "RahkaranAssets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ApproveStatus",
                schema: "Rahkaran",
                table: "RahkaranAssets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedBy",
                schema: "Rahkaran",
                table: "RahkaranAssets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApproverRemarks",
                schema: "Rahkaran",
                table: "RahkaranAssets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveDate",
                schema: "Rahkaran",
                table: "RahkaranAssets");

            migrationBuilder.DropColumn(
                name: "ApproveStatus",
                schema: "Rahkaran",
                table: "RahkaranAssets");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                schema: "Rahkaran",
                table: "RahkaranAssets");

            migrationBuilder.DropColumn(
                name: "ApproverRemarks",
                schema: "Rahkaran",
                table: "RahkaranAssets");
        }
    }
}

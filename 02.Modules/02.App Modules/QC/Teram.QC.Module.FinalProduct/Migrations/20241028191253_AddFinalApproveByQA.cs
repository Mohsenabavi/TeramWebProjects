using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.QC.Module.FinalProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddFinalApproveByQA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FinalApproveByQA",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinalApproveByQADate",
                schema: "QCFP",
                table: "FinalProductNoncompliances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalApproveByQA",
                schema: "QCFP",
                table: "FinalProductNoncompliances");

            migrationBuilder.DropColumn(
                name: "FinalApproveByQADate",
                schema: "QCFP",
                table: "FinalProductNoncompliances");
        }
    }
}

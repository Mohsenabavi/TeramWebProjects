using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    /// <inheritdoc />
    public partial class AddInvitationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InvitationToWorkDate",
                schema: "HR",
                table: "JobApplicants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InvitedBy",
                schema: "HR",
                table: "JobApplicants",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitationToWorkDate",
                schema: "HR",
                table: "JobApplicants");

            migrationBuilder.DropColumn(
                name: "InvitedBy",
                schema: "HR",
                table: "JobApplicants");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teram.HR.Module.Assets.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonnelCodeToAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonnelCode",
                schema: "Rahkaran",
                table: "RahkaranAssets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonnelCode",
                schema: "Rahkaran",
                table: "RahkaranAssets");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITAssetRepo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Switch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Asset_list",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Asset_list");
        }
    }
}

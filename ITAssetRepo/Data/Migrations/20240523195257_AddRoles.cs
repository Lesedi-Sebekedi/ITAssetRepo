using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ITAssetRepo.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22acc876-3ab9-4149-94ed-9b50cd9fd47c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2944cb92-b1e0-416d-abf9-5d25e09d22f5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aaa4603f-fbb9-462d-9cbc-81e0fcd94241");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "22acc876-3ab9-4149-94ed-9b50cd9fd47c", null, "technician", "technician" },
                    { "2944cb92-b1e0-416d-abf9-5d25e09d22f5", null, "asset_team", "asset_team" },
                    { "aaa4603f-fbb9-462d-9cbc-81e0fcd94241", null, "admin", "admin" }
                });
        }
    }
}

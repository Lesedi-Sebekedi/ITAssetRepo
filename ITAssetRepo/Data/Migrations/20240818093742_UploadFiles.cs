using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITAssetRepo.Data.Migrations
{
    /// <inheritdoc />
    public partial class UploadFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Asset_Cost",
                table: "Asset_list",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Acq_Date",
                table: "Asset_list",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "BitlockerFilePath",
                table: "Asset_list",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TechnicalInspectionFilePath",
                table: "Asset_list",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BitlockerFilePath",
                table: "Asset_list");

            migrationBuilder.DropColumn(
                name: "TechnicalInspectionFilePath",
                table: "Asset_list");

            migrationBuilder.AlterColumn<string>(
                name: "Asset_Cost",
                table: "Asset_list",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Acq_Date",
                table: "Asset_list",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}

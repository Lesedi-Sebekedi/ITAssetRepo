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
            migrationBuilder.DropPrimaryKey(
                name: "PK_Asset_list",
                table: "Asset_list");

            migrationBuilder.RenameTable(
                name: "Asset_list",
                newName: "Assets");

            migrationBuilder.AlterColumn<decimal>(
                name: "Asset_Cost",
                table: "Assets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Acq_Date",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "BitlockerFilePath",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TechnicalInspectionFilePath",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assets",
                table: "Assets",
                column: "Asset_Number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Assets",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "BitlockerFilePath",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "TechnicalInspectionFilePath",
                table: "Assets");

            migrationBuilder.RenameTable(
                name: "Assets",
                newName: "Asset_list");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Asset_list",
                table: "Asset_list",
                column: "Asset_Number");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class ContractLeadKeyin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Contracts");

            migrationBuilder.AddColumn<Guid>(
                name: "LeadId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeadId",
                table: "Contracts");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Contracts",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Contracts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "Contracts",
                type: "bit",
                nullable: true);
        }
    }
}

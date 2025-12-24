using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class GiftFieldTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Gifts");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Gifts",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Gifts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateOnly>(
                name: "ExpiredDate",
                table: "Gifts",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Gifts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "ExpiredDate",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Gifts");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Gifts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

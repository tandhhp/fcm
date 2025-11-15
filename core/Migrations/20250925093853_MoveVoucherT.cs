using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class MoveVoucherT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Vouchers_VoucherId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_VoucherId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "VoucherId",
                table: "Contracts");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Vouchers",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "VoucherId",
                table: "Leads",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_VoucherId",
                table: "Leads",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Vouchers_VoucherId",
                table: "Leads",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Vouchers_VoucherId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_VoucherId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "VoucherId",
                table: "Leads");

            migrationBuilder.AddColumn<Guid>(
                name: "VoucherId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_VoucherId",
                table: "Contracts",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Vouchers_VoucherId",
                table: "Contracts",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id");
        }
    }
}

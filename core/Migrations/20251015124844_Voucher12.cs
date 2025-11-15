using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class Voucher12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Vouchers_VoucherId",
                table: "Leads");

            migrationBuilder.RenameColumn(
                name: "VoucherId",
                table: "Leads",
                newName: "Voucher2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Leads_VoucherId",
                table: "Leads",
                newName: "IX_Leads_Voucher2Id");

            migrationBuilder.AddColumn<Guid>(
                name: "Voucher1Id",
                table: "Leads",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_Voucher1Id",
                table: "Leads",
                column: "Voucher1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Vouchers_Voucher1Id",
                table: "Leads",
                column: "Voucher1Id",
                principalTable: "Vouchers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Vouchers_Voucher2Id",
                table: "Leads",
                column: "Voucher2Id",
                principalTable: "Vouchers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Vouchers_Voucher1Id",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Vouchers_Voucher2Id",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_Voucher1Id",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "Voucher1Id",
                table: "Leads");

            migrationBuilder.RenameColumn(
                name: "Voucher2Id",
                table: "Leads",
                newName: "VoucherId");

            migrationBuilder.RenameIndex(
                name: "IX_Leads_Voucher2Id",
                table: "Leads",
                newName: "IX_Leads_VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Vouchers_VoucherId",
                table: "Leads",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id");
        }
    }
}

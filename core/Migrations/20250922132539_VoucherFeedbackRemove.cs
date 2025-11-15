using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class VoucherFeedbackRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadFeedbacks_Vouchers_VoucherId",
                table: "LeadFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_LeadFeedbacks_VoucherId",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "VoucherId",
                table: "LeadFeedbacks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VoucherId",
                table: "LeadFeedbacks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeadFeedbacks_VoucherId",
                table: "LeadFeedbacks",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadFeedbacks_Vouchers_VoucherId",
                table: "LeadFeedbacks",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class SourceIdFromFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "Voucher",
                table: "LeadFeedbacks");

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "LeadFeedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VoucherId",
                table: "LeadFeedbacks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeadFeedbacks_SourceId",
                table: "LeadFeedbacks",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFeedbacks_VoucherId",
                table: "LeadFeedbacks",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadFeedbacks_Sources_SourceId",
                table: "LeadFeedbacks",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadFeedbacks_Vouchers_VoucherId",
                table: "LeadFeedbacks",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadFeedbacks_Sources_SourceId",
                table: "LeadFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadFeedbacks_Vouchers_VoucherId",
                table: "LeadFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_LeadFeedbacks_SourceId",
                table: "LeadFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_LeadFeedbacks_VoucherId",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "VoucherId",
                table: "LeadFeedbacks");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "LeadFeedbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Voucher",
                table: "LeadFeedbacks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

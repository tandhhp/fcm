using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceEvtT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesId",
                table: "LeadFeedbacks");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LeadId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_LeadId",
                table: "Contracts",
                column: "LeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Leads_LeadId",
                table: "Contracts",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Leads_LeadId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_LeadId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "LeadId",
                table: "Contracts");

            migrationBuilder.AddColumn<Guid>(
                name: "SalesId",
                table: "LeadFeedbacks",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}

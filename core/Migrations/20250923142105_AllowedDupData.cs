using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class AllowedDupData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Leads",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Duplicated",
                table: "Leads",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_SalesId",
                table: "Leads",
                column: "SalesId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_TelesaleId",
                table: "Leads",
                column: "TelesaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_AspNetUsers_SalesId",
                table: "Leads",
                column: "SalesId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_AspNetUsers_TelesaleId",
                table: "Leads",
                column: "TelesaleId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_AspNetUsers_SalesId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_AspNetUsers_TelesaleId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_SalesId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_TelesaleId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "Duplicated",
                table: "Leads");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Leads",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}

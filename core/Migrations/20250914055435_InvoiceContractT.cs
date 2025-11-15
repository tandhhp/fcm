using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceContractT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Gifts_GiftId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Leads_LeadId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Vouchers_VoucherId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_LeadId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Evidence",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "LeadId",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "LeadFeedbacks",
                newName: "SalesId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Invoices",
                newName: "SalesId");

            migrationBuilder.AddColumn<Guid>(
                name: "ContractId",
                table: "Invoices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "FileContents",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "FileContents",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FileContents",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "UploadBy",
                table: "FileContents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDate",
                table: "FileContents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "VoucherId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "GiftId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Contracts",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Contacts",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ContractId",
                table: "Invoices",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Gifts_GiftId",
                table: "Contracts",
                column: "GiftId",
                principalTable: "Gifts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Vouchers_VoucherId",
                table: "Contracts",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Contracts_ContractId",
                table: "Invoices",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Gifts_GiftId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Vouchers_VoucherId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Contracts_ContractId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_ContractId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "UploadBy",
                table: "FileContents");

            migrationBuilder.DropColumn(
                name: "UploadDate",
                table: "FileContents");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "SalesId",
                table: "LeadFeedbacks",
                newName: "SellerId");

            migrationBuilder.RenameColumn(
                name: "SalesId",
                table: "Invoices",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Evidence",
                table: "LeadFeedbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Floor",
                table: "LeadFeedbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "FileContents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "FileContents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FileContents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<Guid>(
                name: "VoucherId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "GiftId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LeadId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Contacts",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_LeadId",
                table: "Contracts",
                column: "LeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Gifts_GiftId",
                table: "Contracts",
                column: "GiftId",
                principalTable: "Gifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Leads_LeadId",
                table: "Contracts",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Vouchers_VoucherId",
                table: "Contracts",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

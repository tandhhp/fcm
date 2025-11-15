using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class ToNameT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Leads_LeadId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "TO",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Contracts");

            migrationBuilder.AddColumn<string>(
                name: "ContractCode",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToById",
                table: "Leads",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LeadId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "Contracts",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Contracts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToById",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Leads_LeadId",
                table: "Contracts",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Leads_LeadId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ContractCode",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ToById",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ToById",
                table: "Contracts");

            migrationBuilder.AddColumn<string>(
                name: "TO",
                table: "LeadFeedbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LeadId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Leads_LeadId",
                table: "Contracts",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

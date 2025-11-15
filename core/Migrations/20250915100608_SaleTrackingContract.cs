using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class SaleTrackingContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Campaigns_CampaignId",
                table: "Vouchers");

            migrationBuilder.AlterColumn<int>(
                name: "CampaignId",
                table: "Vouchers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Vouchers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Campaigns_CampaignId",
                table: "Vouchers",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Campaigns_CampaignId",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "SalesId",
                table: "Contracts");

            migrationBuilder.AlterColumn<int>(
                name: "CampaignId",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Campaigns_CampaignId",
                table: "Vouchers",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

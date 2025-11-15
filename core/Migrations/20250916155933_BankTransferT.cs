using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class BankTransferT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cards_CardId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CardId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Benefits",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ContractPrice",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ExpiredTime",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Limit",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Loyalty",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "MaxLoyalty",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Refund",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ServicePrice",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Tier",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "TopUpPoint",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Whynow",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaxLoyalty",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CardId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FrontImage",
                table: "Cards",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Cards",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BackImage",
                table: "Cards",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cards",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CardId",
                table: "Contracts",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Cards_CardId",
                table: "Contracts",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Cards_CardId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_CardId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cards");

            migrationBuilder.AlterColumn<string>(
                name: "FrontImage",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "BackImage",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Benefits",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Quyền lợi chính");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractPrice",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Giá trị hợp đồng");

            migrationBuilder.AddColumn<string>(
                name: "ExpiredTime",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Thời hạn hợp đồng");

            migrationBuilder.AddColumn<string>(
                name: "Limit",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Hạn mức ứng trước");

            migrationBuilder.AddColumn<int>(
                name: "Loyalty",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MaxLoyalty",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Số điểm tối đa");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Cards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Refund",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Ưu đãi hoàn tiền");

            migrationBuilder.AddColumn<string>(
                name: "ServicePrice",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Phí dịch vụ thẻ");

            migrationBuilder.AddColumn<int>(
                name: "Tier",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TopUpPoint",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Điểm Top Up");

            migrationBuilder.AddColumn<string>(
                name: "Whynow",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Whynow");

            migrationBuilder.AddColumn<Guid>(
                name: "CardId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxLoyalty",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CardId",
                table: "AspNetUsers",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cards_CardId",
                table: "AspNetUsers",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id");
        }
    }
}

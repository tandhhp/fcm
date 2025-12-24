using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class GiftFieldTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Gifts_GiftId",
                table: "Contracts");

            migrationBuilder.DropTable(
                name: "ContractGifts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_GiftId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "GiftId",
                table: "Contracts");

            migrationBuilder.AddColumn<Guid>(
                name: "ContractId",
                table: "Gifts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_ContractId",
                table: "Gifts",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Contracts_ContractId",
                table: "Gifts",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Contracts_ContractId",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_ContractId",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Gifts");

            migrationBuilder.AddColumn<Guid>(
                name: "GiftId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContractGifts",
                columns: table => new
                {
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractGifts", x => new { x.ContractId, x.GiftId });
                    table.ForeignKey(
                        name: "FK_ContractGifts_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractGifts_Gifts_GiftId",
                        column: x => x.GiftId,
                        principalTable: "Gifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_GiftId",
                table: "Contracts",
                column: "GiftId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractGifts_GiftId",
                table: "ContractGifts",
                column: "GiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Gifts_GiftId",
                table: "Contracts",
                column: "GiftId",
                principalTable: "Gifts",
                principalColumn: "Id");
        }
    }
}

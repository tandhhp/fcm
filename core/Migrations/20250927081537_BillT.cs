using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class BillT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Leads_LeadId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_LeadId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ContractCode",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "LeadId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Concerns",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FamilyCharacteristics",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HealthHistory",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IdentityAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IdentityDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LoyaltyExpiredDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Personality",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "Contracts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bills_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bills_ContractId",
                table: "Bills",
                column: "ContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "Contracts");

            migrationBuilder.AddColumn<string>(
                name: "ContractCode",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LeadId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Concerns",
                table: "AspNetUsers",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                comment: "Mối quan tâm");

            migrationBuilder.AddColumn<string>(
                name: "FamilyCharacteristics",
                table: "AspNetUsers",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                comment: "Đặc điểm gia đình");

            migrationBuilder.AddColumn<string>(
                name: "HealthHistory",
                table: "AspNetUsers",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                comment: "Tiền sử sức khỏe");

            migrationBuilder.AddColumn<string>(
                name: "IdentityAddress",
                table: "AspNetUsers",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IdentityDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LoyaltyExpiredDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Personality",
                table: "AspNetUsers",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                comment: "Đặc điểm tính cách");

            migrationBuilder.AddColumn<Guid>(
                name: "SellerId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_LeadId",
                table: "Contracts",
                column: "LeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Leads_LeadId",
                table: "Contracts",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id");
        }
    }
}

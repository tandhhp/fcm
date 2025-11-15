using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class SubUserMotivale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubUsers_AspNetUsers_UserId",
                table: "SubUsers");

            migrationBuilder.DropIndex(
                name: "IX_SubUsers_UserId",
                table: "SubUsers");

            migrationBuilder.DropColumn(
                name: "AccountantId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "DosId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "DotId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "TmId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ContractCode2",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "EventTime",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "IsOnsiteTesting",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "HasChange",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CardHolderId",
                table: "Contracts",
                newName: "VoucherId");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Leads",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Leads",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityNumber",
                table: "Leads",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true,
                comment: "DDCN",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "DDCN");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Leads",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobKindId",
                table: "LeadFeedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Contracts",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "GiftId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LeadId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityNumber",
                table: "AspNetUsers",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Avatar",
                table: "AspNetUsers",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Gifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvidenceUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vouchers_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeadFeedbacks_JobKindId",
                table: "LeadFeedbacks",
                column: "JobKindId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_GiftId",
                table: "Contracts",
                column: "GiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_LeadId",
                table: "Contracts",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_VoucherId",
                table: "Contracts",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DistrictId",
                table: "AspNetUsers",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceNumber",
                table: "Invoices",
                column: "InvoiceNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_CampaignId",
                table: "Vouchers",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Districts_DistrictId",
                table: "AspNetUsers",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_LeadFeedbacks_JobKinds_JobKindId",
                table: "LeadFeedbacks",
                column: "JobKindId",
                principalTable: "JobKinds",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Districts_DistrictId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Gifts_GiftId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Leads_LeadId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Vouchers_VoucherId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadFeedbacks_JobKinds_JobKindId",
                table: "LeadFeedbacks");

            migrationBuilder.DropTable(
                name: "Gifts");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_LeadFeedbacks_JobKindId",
                table: "LeadFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_GiftId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_LeadId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_VoucherId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DistrictId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobKindId",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "GiftId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "LeadId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "VoucherId",
                table: "Contracts",
                newName: "CardHolderId");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityNumber",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true,
                comment: "DDCN",
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12,
                oldNullable: true,
                oldComment: "DDCN");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AccountantId",
                table: "Leads",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DosId",
                table: "Leads",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DotId",
                table: "Leads",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TmId",
                table: "Leads",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractCode2",
                table: "LeadFeedbacks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EventDate",
                table: "LeadFeedbacks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EventTime",
                table: "LeadFeedbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnsiteTesting",
                table: "LeadFeedbacks",
                type: "bit",
                nullable: true,
                comment: "Xét nghiệm tại chỗ");

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "LeadFeedbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Avatar",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasChange",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RefId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubUsers_UserId",
                table: "SubUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubUsers_AspNetUsers_UserId",
                table: "SubUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

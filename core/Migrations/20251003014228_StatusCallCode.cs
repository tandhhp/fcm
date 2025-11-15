using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class StatusCallCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardHolderQueues");

            migrationBuilder.DropTable(
                name: "LeadProcesses");

            migrationBuilder.DropTable(
                name: "TopupEvidences");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CallStatuses",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillNumber",
                table: "Bills",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "CallStatuses");

            migrationBuilder.DropColumn(
                name: "BillNumber",
                table: "Bills");

            migrationBuilder.CreateTable(
                name: "CardHolderQueues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardHolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardHolderQueues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeadProcesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadProcesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TopupEvidences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopupEvidences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopupEvidences_UserTopups_TopupId",
                        column: x => x.TopupId,
                        principalTable: "UserTopups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopupEvidences_TopupId",
                table: "TopupEvidences",
                column: "TopupId");
        }
    }
}

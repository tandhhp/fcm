using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class StaffIdCallWebhookLogField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StaffId",
                table: "CallWebhookLogs",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "CallWebhookLogs");
        }
    }
}

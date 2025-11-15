using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class LeadHistoryUpdateF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "CheckinTime",
                table: "LeadHistories",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CheckoutTime",
                table: "LeadHistories",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "LeadHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "LeadHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "LeadHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToById",
                table: "LeadHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransportId",
                table: "LeadHistories",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckinTime",
                table: "LeadHistories");

            migrationBuilder.DropColumn(
                name: "CheckoutTime",
                table: "LeadHistories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "LeadHistories");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "LeadHistories");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "LeadHistories");

            migrationBuilder.DropColumn(
                name: "ToById",
                table: "LeadHistories");

            migrationBuilder.DropColumn(
                name: "TransportId",
                table: "LeadHistories");
        }
    }
}

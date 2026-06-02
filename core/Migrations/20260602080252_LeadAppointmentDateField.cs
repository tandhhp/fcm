using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class LeadAppointmentDateField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdentityNumber",
                table: "Leads",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true,
                comment: "Số CCCD",
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12,
                oldNullable: true,
                oldComment: "DDCN");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDate",
                table: "Leads",
                type: "datetime2",
                nullable: true,
                comment: "Ngày tạo lịch hẹn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                table: "Leads");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityNumber",
                table: "Leads",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true,
                comment: "DDCN",
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12,
                oldNullable: true,
                oldComment: "Số CCCD");
        }
    }
}

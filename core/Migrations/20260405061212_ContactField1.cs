using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class ContactField1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CallStatusId",
                table: "Contacts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraStatus",
                table: "Contacts",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FollowUpdate",
                table: "Contacts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CallStatusId",
                table: "Contacts",
                column: "CallStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_CallStatuses_CallStatusId",
                table: "Contacts",
                column: "CallStatusId",
                principalTable: "CallStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_CallStatuses_CallStatusId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_CallStatusId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "CallStatusId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "ExtraStatus",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "FollowUpdate",
                table: "Contacts");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class SourceContactT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetaData",
                table: "Contacts");

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "Contacts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_SourceId",
                table: "Contacts",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Sources_SourceId",
                table: "Contacts",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Sources_SourceId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_SourceId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "Contacts");

            migrationBuilder.AddColumn<string>(
                name: "MetaData",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class TypeOfDataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Overwrite",
                table: "Sources",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Protected",
                table: "Sources",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SourceId",
                table: "Teams",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Sources_SourceId",
                table: "Teams",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Sources_SourceId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_SourceId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Overwrite",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "Protected",
                table: "Sources");
        }
    }
}

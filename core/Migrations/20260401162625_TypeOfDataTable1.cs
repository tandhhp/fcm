using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class TypeOfDataTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Sources",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Sources");

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "Teams",
                type: "int",
                nullable: true);

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
    }
}

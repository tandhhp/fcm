using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class TypeOfDataTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeOfDataId",
                table: "Sources",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TypeOfDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfDatas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sources_TypeOfDataId",
                table: "Sources",
                column: "TypeOfDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_TypeOfDatas_TypeOfDataId",
                table: "Sources",
                column: "TypeOfDataId",
                principalTable: "TypeOfDatas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sources_TypeOfDatas_TypeOfDataId",
                table: "Sources");

            migrationBuilder.DropTable(
                name: "TypeOfDatas");

            migrationBuilder.DropIndex(
                name: "IX_Sources_TypeOfDataId",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "TypeOfDataId",
                table: "Sources");
        }
    }
}

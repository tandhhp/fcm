using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class GroupDataCallCenter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CallCenterId",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupDataId",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CallCenters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallCenters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDatas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CallCenterId",
                table: "Teams",
                column: "CallCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_GroupDataId",
                table: "Teams",
                column: "GroupDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_CallCenters_CallCenterId",
                table: "Teams",
                column: "CallCenterId",
                principalTable: "CallCenters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_GroupDatas_GroupDataId",
                table: "Teams",
                column: "GroupDataId",
                principalTable: "GroupDatas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_CallCenters_CallCenterId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_GroupDatas_GroupDataId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "CallCenters");

            migrationBuilder.DropTable(
                name: "GroupDatas");

            migrationBuilder.DropIndex(
                name: "IX_Teams_CallCenterId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_GroupDataId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CallCenterId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "GroupDataId",
                table: "Teams");
        }
    }
}

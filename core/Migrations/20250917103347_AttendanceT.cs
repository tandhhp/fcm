using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class AttendanceT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableStatus",
                table: "LeadHistories");

            migrationBuilder.AddColumn<int>(
                name: "AttendanceId",
                table: "Leads",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttendanceId",
                table: "LeadHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransportId",
                table: "LeadFeedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    COMRate = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leads_AttendanceId",
                table: "Leads",
                column: "AttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadFeedbacks_TransportId",
                table: "LeadFeedbacks",
                column: "TransportId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadFeedbacks_Transports_TransportId",
                table: "LeadFeedbacks",
                column: "TransportId",
                principalTable: "Transports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Attendances_AttendanceId",
                table: "Leads",
                column: "AttendanceId",
                principalTable: "Attendances",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadFeedbacks_Transports_TransportId",
                table: "LeadFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Attendances_AttendanceId",
                table: "Leads");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Leads_AttendanceId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_LeadFeedbacks_TransportId",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "AttendanceId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "AttendanceId",
                table: "LeadHistories");

            migrationBuilder.DropColumn(
                name: "TransportId",
                table: "LeadFeedbacks");

            migrationBuilder.AddColumn<string>(
                name: "TableStatus",
                table: "LeadHistories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnuseFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "ContractAmount",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "ContractCode",
                table: "LeadFeedbacks");

            migrationBuilder.DropColumn(
                name: "TableStatus",
                table: "LeadFeedbacks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AmountPaid",
                table: "LeadFeedbacks",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ContractAmount",
                table: "LeadFeedbacks",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContractCode",
                table: "LeadFeedbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TableStatus",
                table: "LeadFeedbacks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

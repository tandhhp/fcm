using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class SourceDefaultUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Personality",
                table: "AspNetUsers",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                comment: "Đặc điểm tính cách",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Đặc điểm tính cách");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityAddress",
                table: "AspNetUsers",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HealthHistory",
                table: "AspNetUsers",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                comment: "Tiền sử sức khỏe",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Tiền sử sức khỏe");

            migrationBuilder.AlterColumn<string>(
                name: "FamilyCharacteristics",
                table: "AspNetUsers",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                comment: "Đặc điểm gia đình",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Đặc điểm gia đình");

            migrationBuilder.AlterColumn<string>(
                name: "Concerns",
                table: "AspNetUsers",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                comment: "Mối quan tâm",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Mối quan tâm");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SourceId",
                table: "AspNetUsers",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sources_SourceId",
                table: "AspNetUsers",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sources_SourceId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SourceId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Personality",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Đặc điểm tính cách",
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048,
                oldNullable: true,
                oldComment: "Đặc điểm tính cách");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityAddress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HealthHistory",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Tiền sử sức khỏe",
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048,
                oldNullable: true,
                oldComment: "Tiền sử sức khỏe");

            migrationBuilder.AlterColumn<string>(
                name: "FamilyCharacteristics",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Đặc điểm gia đình",
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048,
                oldNullable: true,
                oldComment: "Đặc điểm gia đình");

            migrationBuilder.AlterColumn<string>(
                name: "Concerns",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Mối quan tâm",
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048,
                oldNullable: true,
                oldComment: "Mối quan tâm");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048,
                oldNullable: true);
        }
    }
}

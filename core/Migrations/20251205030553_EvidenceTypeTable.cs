using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class EvidenceTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evidence_Contracts_ContractId",
                table: "Evidence");

            migrationBuilder.DropForeignKey(
                name: "FK_Evidence_EvidenceType_EvidenceTypeId",
                table: "Evidence");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EvidenceType",
                table: "EvidenceType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Evidence",
                table: "Evidence");

            migrationBuilder.RenameTable(
                name: "EvidenceType",
                newName: "EvidenceTypes");

            migrationBuilder.RenameTable(
                name: "Evidence",
                newName: "Evidences");

            migrationBuilder.RenameIndex(
                name: "IX_Evidence_EvidenceTypeId",
                table: "Evidences",
                newName: "IX_Evidences_EvidenceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Evidence_ContractId",
                table: "Evidences",
                newName: "IX_Evidences_ContractId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvidenceTypes",
                table: "EvidenceTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evidences",
                table: "Evidences",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evidences_Contracts_ContractId",
                table: "Evidences",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Evidences_EvidenceTypes_EvidenceTypeId",
                table: "Evidences",
                column: "EvidenceTypeId",
                principalTable: "EvidenceTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evidences_Contracts_ContractId",
                table: "Evidences");

            migrationBuilder.DropForeignKey(
                name: "FK_Evidences_EvidenceTypes_EvidenceTypeId",
                table: "Evidences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EvidenceTypes",
                table: "EvidenceTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Evidences",
                table: "Evidences");

            migrationBuilder.RenameTable(
                name: "EvidenceTypes",
                newName: "EvidenceType");

            migrationBuilder.RenameTable(
                name: "Evidences",
                newName: "Evidence");

            migrationBuilder.RenameIndex(
                name: "IX_Evidences_EvidenceTypeId",
                table: "Evidence",
                newName: "IX_Evidence_EvidenceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Evidences_ContractId",
                table: "Evidence",
                newName: "IX_Evidence_ContractId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvidenceType",
                table: "EvidenceType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evidence",
                table: "Evidence",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evidence_Contracts_ContractId",
                table: "Evidence",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Evidence_EvidenceType_EvidenceTypeId",
                table: "Evidence",
                column: "EvidenceTypeId",
                principalTable: "EvidenceType",
                principalColumn: "Id");
        }
    }
}

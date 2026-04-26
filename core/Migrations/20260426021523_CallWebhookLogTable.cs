using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waffle.Migrations
{
    /// <inheritdoc />
    public partial class CallWebhookLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CallWebhookLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Application = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Billsec = table.Column<int>(type: "int", nullable: true),
                    CallId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CampaignUuid = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Direction = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Domain = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DomainUuid = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    FromNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Hotline = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LeadUuid = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PressKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ReceiveDest = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    RecordingUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    RefId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SipCallId = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    SipHangupDisposition = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    State = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    TimeAnswered = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeEnded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeStarted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallWebhookLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallWebhookLogs");
        }
    }
}

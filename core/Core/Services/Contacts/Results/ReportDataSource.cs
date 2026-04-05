using System.ComponentModel;

namespace Waffle.Core.Services.Contacts.Results;

public class ReportDataSource
{
    // ==========================================
    // THÔNG TIN CHUNG
    // ==========================================
    [DisplayName("Source Group")]
    public string? SourceGroup { get; set; }

    [DisplayName("Source Name")]
    public string? SourceName { get; set; }

    // ==========================================
    // TYPE OF DATA
    // ==========================================
    [DisplayName("Contact Import")]
    public int? ContactImport { get; set; }

    [DisplayName("Contact Start Case")]
    public int? ContactStartCase { get; set; }

    [DisplayName("Total (Type of Data)")]
    public int? TotalTypeOfData { get; set; }

    // ==========================================
    // TOTAL NOT CONTACTED
    // ==========================================
    [DisplayName("0. Tele not update")]
    public int? TeleNotUpdate { get; set; }

    [DisplayName("1. Temporary locked/Wrong number/Knm")]
    public int? TempLockedWrongNumber { get; set; }

    [DisplayName("Total (1)")]
    public int? Total1 { get; set; }

    // ==========================================
    // TOTAL CONTACTED
    // ==========================================
    [DisplayName("2. Not Enough Qualify")]
    public int? NotEnoughQualify { get; set; }

    [DisplayName("3. Meet Require")]
    public int? MeetRequire { get; set; }

    [DisplayName("4. Refuse to talk")]
    public int? RefuseToTalk { get; set; }

    [DisplayName("5. Location")]
    public int? Location { get; set; }

    [DisplayName("Total (2)")]
    public int? Total2 { get; set; }

    // ==========================================
    // TOTAL INVITE
    // ==========================================
    [DisplayName("CF1")]
    public int? CF1 { get; set; }

    [DisplayName("Consider")]
    public int? Consider { get; set; }

    // ==========================================
    // TỈ LỆ & KẾT QUẢ
    // ==========================================
    [DisplayName("%CF/Total Contacted")]
    public double? PercentCFTotalContacted { get; set; }

    [DisplayName("Showup")]
    public int? Showup { get; set; }

    [DisplayName("%Showup/CF")]
    public double? PercentShowupCF { get; set; }

    [DisplayName("Deal")]
    public int? Deal { get; set; }

    [DisplayName("%Deal/Showup")]
    public double? PercentDealShowup { get; set; }
}
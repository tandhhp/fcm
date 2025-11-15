using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Waffle.Entities.Contracts;

namespace Waffle.Entities.Payments;

[Index(nameof(InvoiceNumber))]
public class Invoice : BaseEntity
{
    [StringLength(256)]
    public string InvoiceNumber { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    [Column(TypeName = "money")]
    public decimal Amount { get; set; }
    public Guid CreatedBy { get; set; }
    [StringLength(2048)]
    public string? EvidenceUrl { get; set; }
    public InvoiceStatus Status { get; set; }
    [ForeignKey(nameof(Contract))]
    public Guid ContractId { get; set; }
    public Guid SalesId { get; set; }
    public string? Note { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public Contract? Contract { get; set; }
}

public enum InvoiceStatus
{
    [Display(Name = "Chờ duyệt")]
    Pending,
    [Display(Name = "Đã duyệt")]
    Approved,
    [Display(Name = "Từ chối")]
    Rejected,
    [Display(Name = "Hủy")]
    Cancelled
}

public enum PaymentMethod
{
    [Display(Name = "Chuyển khoản")]
    BankTransfer,
    [Display(Name = "Quẹt thẻ")]
    Card,
    [Display(Name = "Tiền mặt")]
    Cash
}
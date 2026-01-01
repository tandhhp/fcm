namespace Waffle.Core.Services.Contracts.Args;

public class UploadEvidencesArgs
{
    public List<IFormFile>? Files { get; set; }
    public int EvidenceTypeId { get; set; }
    public Guid ContractId { get; set; }
    public Guid? InvoiceId { get; set; }
}

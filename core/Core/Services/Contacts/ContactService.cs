using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Waffle.Core.Constants;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IRepository.Calls;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Contacts.Args;
using Waffle.Core.Services.Contacts.Filters;
using Waffle.Core.Services.Contacts.Models;
using Waffle.Core.Services.Contacts.Results;
using Waffle.Core.Services.Leads.Args;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Contacts;
using Waffle.Models;
using Waffle.Models.Filters;

namespace Waffle.Core.Services.Contacts;

public class ContactService(IContactRepository _contactRepository, ApplicationDbContext _context, IWebHostEnvironment _env, ICallStatusRepository _callStatusRepository, IProvinceService _provinceService, ISourceService _sourceService, IDistrictService _districtService, ILogService _logService, UserManager<ApplicationUser> _userManager, IHCAService _hcaService, ILeadService _leadService) : IContactService
{
    public async Task<TResult> BlockAsync(BlockContactArgs args)
    {
        var contact = await _contactRepository.FindAsync(args.Id);
        if (contact is null) return TResult.Failed("Không tìm thấy liên hệ!");
        contact.Status = ContactStatus.Blacklisted;
        contact.Note = args.Note;
        await _contactRepository.UpdateAsync(contact);
        await _logService.AddAsync($"Chặn liên hệ {contact.Name} - {contact.PhoneNumber}");
        return TResult.Success;
    }

    public async Task<TResult> CreateContactAsync(CreateContactArgs args)
    {
        try
        {
            if (!PhoneNumberValidator.IsValidVietnamPhoneNumber(args.PhoneNumber)) return TResult.Failed("Số điện thoại không hợp lệ");
            if (await _contactRepository.IsPhoneExistAsync(args.PhoneNumber)) return TResult.Failed("Số điện thoại đã tồn tại");
            if (args.UserId != null && !await _userManager.Users.AnyAsync(x => x.Id == args.UserId)) return TResult.Failed("Người dùng không tồn tại");

            await _contactRepository.AddAsync(new Contact
            {
                Name = args.Name,
                PhoneNumber = args.PhoneNumber,
                Email = args.Email,
                Status = ContactStatus.New,
                CreatedDate = DateTime.Now,
                UserId = args.UserId,
                CreatedBy = _hcaService.GetUserId(),
                DistrictId = args.DistrictId,
                JobKindId = args.JobKindId,
                MarriedStatus = args.MarriedStatus,
                Note = args.Note,
                Gender = args.Gender,
                TransportId = args.TransportId
            });
            return TResult.Success;
        }
        catch (Exception ex)
        {
            return TResult.Failed(ex.ToString());
        }
    }

    public Task<Contact?> FindAsync(Guid id) => _contactRepository.FindAsync(id);

    public Task<ListResult<object>> GetBlacklistAsync(BlacklistFilterOptions filterOptions) => _contactRepository.GetBlacklistAsync(filterOptions);

    public async Task<TResult<object>> DetailAsync(Guid id)
    {
        var contact = await _contactRepository.FindAsync(id);
        if (contact is null) return TResult<object>.Failed("Không tìm thấy liên hệ!");
        var district = new District();
        var province = new Province();
        if (contact.DistrictId.HasValue)
        {
            district = await _districtService.FindAsync(contact.DistrictId.GetValueOrDefault());
            if (district is null) return TResult<object>.Failed("Không tìm thấy xã/phường!");
            province = await _provinceService.FindAsync(district.ProvinceId);
            if (province is null) return TResult<object>.Failed("Không tìm thấy tỉnh/thành phố!");
        }
        var user = new ApplicationUser();
        var telesalesManager = new ApplicationUser();
        if (contact.UserId != null)
        {
            user = await _userManager.FindByIdAsync(contact.UserId.GetValueOrDefault().ToString());
            if (user is null) return TResult<object>.Failed("Không tìm thấy người dùng!");
            if (user.TmId != null)
            {
                telesalesManager = await _userManager.FindByIdAsync(user.TmId.GetValueOrDefault().ToString());
                if (telesalesManager is null) return TResult<object>.Failed("Không tìm thấy quản lý telesale!");
            }
        }
        return TResult<object>.Ok(new
        {
            contact.Id,
            contact.Name,
            contact.PhoneNumber,
            contact.Email,
            contact.Address,
            contact.Status,
            contact.Note,
            contact.DistrictId,
            contact.JobKindId,
            contact.MarriedStatus,
            contact.ModifiedBy,
            contact.ModifiedDate,
            contact.CreatedDate,
            contact.UserId,
            DistrictName = district.Name,
            ProvinceId = contact.DistrictId.HasValue ? district?.ProvinceId : null,
            ProvinceName = province.Name,
            contact.Gender,
            user.TeamId,
            user.TmId,
            TelesalesManagerName = telesalesManager.Name,
            contact.SourceId
        });
    }

    public async Task<TResult> UpdateAsync(ContactUpdateArgs args)
    {
        var data = await _contactRepository.FindAsync(args.Id);
        if (data is null) return TResult.Failed("Không tìm thấy liên hệ!");
        if (!PhoneNumberValidator.IsValidVietnamPhoneNumber(args.PhoneNumber)) return TResult.Failed("Số điện thoại không hợp lệ");
        data.Name = args.Name;
        data.PhoneNumber = args.PhoneNumber;
        data.Email = args.Email;
        data.DistrictId = args.DistrictId;
        data.JobKindId = args.JobKindId;
        data.MarriedStatus = args.MarriedStatus;
        data.Note = args.Note;
        data.SourceId = args.SourceId;
        data.Gender = args.Gender;
        data.TransportId = args.TransportId;
        data.ModifiedDate = DateTime.Now;
        data.ModifiedBy = _hcaService.GetUserId();
        await _logService.AddAsync($"Cập nhật liên hệ {data.Name} - {data.PhoneNumber}");
        await _contactRepository.UpdateAsync(data);
        return TResult.Success;
    }

    public async Task<TResult> CreateAsync(ContactCreateArgs args)
    {
        try
        {
            if (!PhoneNumberValidator.IsValidVietnamPhoneNumber(args.PhoneNumber)) return TResult.Failed("Số điện thoại không hợp lệ");
            if (await _contactRepository.IsPhoneExistAsync(args.PhoneNumber)) return TResult.Failed("Số điện thoại đã tồn tại");
            if (args.UserId != null && !await _userManager.Users.AnyAsync(x => x.Id == args.UserId)) return TResult.Failed("Người dùng không tồn tại");

            var contact = new Contact
            {
                Name = args.Name,
                PhoneNumber = args.PhoneNumber,
                Email = args.Email,
                Status = ContactStatus.New,
                CreatedDate = DateTime.Now,
                UserId = args.UserId,
                CreatedBy = _hcaService.GetUserId(),
                DistrictId = args.DistrictId,
                JobKindId = args.JobKindId,
                MarriedStatus = args.MarriedStatus,
                Note = args.Note,
                Gender = args.Gender,
                TransportId = args.TransportId,
                SourceId = args.SourceId
            };
            if (_hcaService.IsUserInRole(RoleName.Telesale))
            {
                contact.UserId = _hcaService.GetUserId();
            }
            await _logService.AddAsync($"Tạo mới liên hệ {contact.Name} - {contact.PhoneNumber}");
            await _contactRepository.AddAsync(contact);
            return TResult.Success;
        }
        catch (Exception ex)
        {
            await _logService.ExceptionAsync(ex);
            return TResult.Failed(ex.ToString());
        }
    }

    public async Task<TResult> BookAsync(ContactBookArgs args)
    {
        try
        {
            if (args.EventDate.Date < DateTime.Now.AddDays(1)) return TResult.Failed("Ngày sự kiện không hợp lệ!");
            var contact = await _contactRepository.FindAsync(args.Id);
            if (contact is null) return TResult.Failed("Không tìm thấy liên hệ!");
            if (string.IsNullOrEmpty(contact.PhoneNumber)) return TResult.Failed("Liên hệ chưa có số điện thoại!");
            var lead = await _leadService.FindByPhoneNumberAsync(contact.PhoneNumber);
            if (lead != null && !lead.Duplicated) return TResult.Failed($"Liên hệ đã có lịch hẹn vào ngày {lead.EventDate:dd-MM-yyyy}!");
            if (contact.UserId == null) return TResult.Failed("Liên hệ chưa có người phụ trách!");
            var telesales = await _userManager.FindByIdAsync(contact.UserId.GetValueOrDefault().ToString());
            if (telesales is null) return TResult.Failed("Người phụ trách không tồn tại!");
            await _leadService.AddAsync(new LeadCreateArgs
            {
                Name = contact.Name,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
                EventDate = args.EventDate,
                EventId = args.EventId,
                Gender = contact.Gender,
                Note = args.Note,
                TelesalesId = contact.UserId,
                BranchId = telesales.BranchId
            });
            return TResult.Success;
        }
        catch (Exception ex)
        {
            return TResult.Failed(ex.ToString());
        }
    }

    public Task<ListResult<dynamic>> ListContactAsync(ContactFilterOptions filterOptions) => _contactRepository.ListAsync(filterOptions);

    public async Task<TResult> ImportAsync(ContactImportArgs args)
    {
        try
        {
            if (args.File is null || args.File.Length == 0) return TResult.Failed("File không hợp lệ!");
            var source = await _sourceService.FindAsync(args.SourceId);
            if (source is null) return TResult.Failed("Nguồn không tồn tại!");
            var phoneNumbers = await _contactRepository.AllPhoneNumbersAsync();
            var contacts = new List<Contact>();
            using var pgk = new ExcelPackage(args.File.OpenReadStream());
            var worksheet = pgk.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;
            var leads = await _context.Leads.Select(x => new
            {
                x.PhoneNumber,
                x.EventDate
            }).Distinct().ToListAsync();

            var errorRows = new List<ContactImportErrorRow>();

            for (int row = 2; row <= rowCount; row++)
            {
                var name = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                if (string.IsNullOrEmpty(name)) continue;
                var phoneNumber = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                if (string.IsNullOrEmpty(phoneNumber)) continue;
                if (!PhoneNumberValidator.IsValidVietnamPhoneNumber(phoneNumber)) return TResult.Failed($"Dòng {row}: Số điện thoại không hợp lệ!");
                var email = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                var districtName = worksheet.Cells[row, 4].Value?.ToString()?.Trim();
                var jobKindName = worksheet.Cells[row, 5].Value?.ToString()?.Trim();
                var marriedStatusString = worksheet.Cells[row, 6].Value?.ToString()?.Trim();
                var genderString = worksheet.Cells[row, 7].Value?.ToString()?.Trim();
                var note = worksheet.Cells[row, 10].Value?.ToString()?.Trim();
                var district = await _districtService.FindByNameAsync(districtName);
                int? districtId = district?.Id;
                MarriedStatus? marriedStatus = marriedStatusString?.ToLower() switch
                {
                    "độc thân" => MarriedStatus.Single,
                    "đã kết hôn" => MarriedStatus.Married,
                    _ => null
                };
                var name2 = worksheet.Cells[row, 8].Value?.ToString()?.Trim();
                var phoneNumber2 = worksheet.Cells[row, 9].Value?.ToString()?.Trim();

                if (phoneNumbers.Any(x => x == phoneNumber) || leads.Any(x => x.PhoneNumber == phoneNumber))
                {
                    var existingLead = leads.FirstOrDefault(x => x.PhoneNumber == phoneNumber);
                    var existingContact = await _contactRepository.FindByPhoneNumberAsync(phoneNumber);
                    if (existingContact is null) continue;
                    var callStatusName = string.Empty;
                    if (existingContact.CallStatusId.HasValue)
                    {
                        var callStatus = await _callStatusRepository.FindAsync(existingContact.CallStatusId);
                        callStatusName = callStatus?.Name ?? string.Empty;
                    }
                    var sourceName = string.Empty;
                    if (existingContact.SourceId.HasValue)
                    {
                        var src = await _sourceService.FindAsync(existingContact.SourceId.Value);
                        sourceName = src?.Name ?? string.Empty;
                    }
                    errorRows.Add(new ContactImportErrorRow
                    {
                        Name = name,
                        PhoneNumber = phoneNumber,
                        Email = email,
                        Address = districtName,
                        JobTitle = jobKindName,
                        MarriedStatus = marriedStatusString,
                        Gender = genderString,
                        Name2 = name2,
                        PhoneNumber2 = phoneNumber2,
                        CallStatus = callStatusName,
                        Note = $"Số điện thoại đã tồn tại trong hệ thống",
                        IsBlackList = existingContact.Status == ContactStatus.Blacklisted,
                        SourceName = sourceName,
                        CheckinNote = existingLead != null ? $"Đã có lịch hẹn vào ngày {existingLead.EventDate:dd-MM-yyyy}" : string.Empty
                    });
                    continue;
                }
                var contact = new Contact
                {
                    Name = name,
                    PhoneNumber = phoneNumber,
                    Email = email,
                    Status = ContactStatus.New,
                    CreatedDate = DateTime.Now,
                    CreatedBy = _hcaService.GetUserId(),
                    DistrictId = districtId,
                    JobKindId = null,
                    MarriedStatus = marriedStatus,
                    Note = note,
                    Gender = genderString?.ToLower() switch
                    {
                        "nam" => false,
                        "nữ" => true,
                        _ => null
                    },
                    SourceId = source.Id,
                    UserId = args.TeleId,
                    Name2 = name2,
                    PhoneNumber2 = phoneNumber2
                };
                contacts.Add(contact);
            }
            await _logService.AddAsync($"Nhập khẩu {contacts.Count} liên hệ");
            await _contactRepository.AddRangeAsync(contacts);
            var errorDownloadLink = string.Empty;
            if (errorRows.Count > 0)
            {
                var fileName = $"contact-import-errors-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                var filePath = Path.Combine(_env.WebRootPath, "errors", fileName);
                using var errorPkg = new ExcelPackage();
                var ws = errorPkg.Workbook.Worksheets.Add("Errors");

                ws.Cells[1, 1].Value = "Name";
                ws.Cells[1, 2].Value = "PhoneNumber";
                ws.Cells[1, 3].Value = "Email";
                ws.Cells[1, 4].Value = "Address";
                ws.Cells[1, 5].Value = "JobTitle";
                ws.Cells[1, 6].Value = "MarriedStatus";
                ws.Cells[1, 7].Value = "Gender";
                ws.Cells[1, 8].Value = "Name2";
                ws.Cells[1, 9].Value = "PhoneNumber2";
                ws.Cells[1, 10].Value = "CallStatus";
                ws.Cells[1, 11].Value = "SourceName";
                ws.Cells[1, 12].Value = "CheckinNote";
                ws.Cells[1, 13].Value = "BlackList";
                ws.Cells[1, 14].Value = "Note";

                for (int i = 0; i < errorRows.Count; i++)
                {
                    var row = i + 2;
                    var item = errorRows[i];
                    ws.Cells[row, 1].Value = item.Name;
                    ws.Cells[row, 2].Value = item.PhoneNumber;
                    ws.Cells[row, 3].Value = item.Email;
                    ws.Cells[row, 4].Value = item.Address;
                    ws.Cells[row, 5].Value = item.JobTitle;
                    ws.Cells[row, 6].Value = item.MarriedStatus;
                    ws.Cells[row, 7].Value = item.Gender;
                    ws.Cells[row, 8].Value = item.Name2;
                    ws.Cells[row, 9].Value = item.PhoneNumber2;
                    ws.Cells[row, 10].Value = item.CallStatus;
                    ws.Cells[row, 11].Value = item.SourceName;
                    ws.Cells[row, 12].Value = item.CheckinNote;
                    ws.Cells[row, 13].Value = item.IsBlackList ? "Có" : "Không";
                    ws.Cells[row, 14].Value = item.Note;
                }
                var cells = ws.Cells[1, 1, errorRows.Count + 1, 14];
                cells.AutoFitColumns();
                cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                await errorPkg.SaveAsAsync(new FileInfo(filePath));
                errorDownloadLink = $"{_hcaService.Request()?.Scheme}://{_hcaService.Request()?.Host}/errors/{fileName}";
                await _logService.AddAsync($"Xuất file lỗi import: {errorDownloadLink}");
            }

            return TResult.Ok(errorDownloadLink);
        }
        catch (Exception ex)
        {
            await _logService.ExceptionAsync(ex);
            return TResult.Failed(ex.ToString());
        }
    }

    public Task<ListResult<object>> GetUnassignedListAsync(UnassignedFilterOptions filterOptions) => _contactRepository.GetUnassignedListAsync(filterOptions);

    public async Task<TResult> AssignSourceAsync(ContactAssignSourceArgs args)
    {
        if (args.NumberOfContact < 1) return TResult.Failed("Số lượng liên hệ phải lớn hơn 0!");
        var tele = await _userManager.FindByIdAsync(args.TelesalesId.ToString());
        if (tele is null) return TResult.Failed("Người dùng không tồn tại!");
        var contacts = await _contactRepository.GetUnassignedContactsAsync(args.NumberOfContact, args.SourceId);
        if (contacts.Count == 0) return TResult.Failed("Không còn liên hệ nào để phân bổ!");
        foreach (var contact in contacts)
        {
            contact.UserId = tele.Id;
            contact.ModifiedDate = DateTime.Now;
            contact.ModifiedBy = _hcaService.GetUserId();
            _contactRepository.Update(contact);
        }
        await _contactRepository.SaveChangesAsync();
        await _logService.AddAsync($"Phân bổ {contacts.Count} liên hệ cho {tele.Name}");
        return TResult.Success;
    }

    public async Task<TResult> Confirm1Async(Guid id)
    {
        var contact = await _contactRepository.FindAsync(id);
        if (contact is null) return TResult.Failed("Không tìm thấy liên hệ!");
        contact.Confirm1 = !contact.Confirm1;
        contact.ModifiedDate = DateTime.Now;
        contact.ModifiedBy = _hcaService.GetUserId();
        await _contactRepository.UpdateAsync(contact);
        return TResult.Success;
    }

    public async Task<TResult> Confirm2Async(UpdateConfirm2Args args)
    {
        var contact = await _contactRepository.FindAsync(args.ContactId);
        if (contact is null) return TResult.Failed("Không tìm thấy liên hệ!");
        contact.Confirm2Status = args.Confirm2Status;
        await _logService.AddAsync($"Cập nhật xác nhận 2 cho liên hệ {contact.Name} - {contact.PhoneNumber} thành {EnumHelper.GetDisplayName(args.Confirm2Status)}");
        await _contactRepository.UpdateAsync(contact);
        return TResult.Success;
    }

    public Task<ListResult<object>> NeedConfirmsAsync(ContactFilterOptions filterOptions) => _contactRepository.NeedConfirmsAsync(filterOptions);

    public Task<TResult<object>> GetTmrReportAsync() => _contactRepository.GetTmrReportAsync();

    public Task<ListResult<object>> DialedCallsAsync(ContactFilterOptions filterOptions) => _contactRepository.DialedCallsAsync(filterOptions);

    public Task<TResult> GetReportDataSourceAsync(ReportDataSourceFilterOptions filterOptions) => _contactRepository.GetReportDataSourceAsync(filterOptions);

    public Task<TResult<byte[]?>> ExportReportDataSourceAsync(ReportDataSourceFilterOptions filterOptions) => _contactRepository.ExportReportDataSourceAsync(filterOptions);

    public Task<TResult> GetTmrDataReportAsync(TmrDataReportFilterOptions filterOptions) => _contactRepository.GetTmrDataReportAsync(filterOptions);

    public Task<TResult<byte[]?>> ExportTmrDataReportAsync(TmrDataReportFilterOptions filterOptions) => _contactRepository.ExportTmrDataReportAsync(filterOptions);

    public Task<TResult<byte[]?>> ExportMultipleAssignReportAsync(MultipleAssignReportFilterOptions filterOptions) => _contactRepository.ExportMultipleAssignReportAsync(filterOptions);

    public Task<TResult> GetMultipleAssignReportAsync(MultipleAssignReportFilterOptions filterOptions) => _contactRepository.GetMultipleAssignReportAsync(filterOptions);
}

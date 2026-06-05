using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Waffle.Core.Constants;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IRepository.Calls;
using Waffle.Core.Interfaces.IRepository.Leads;
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

public class ContactService(IContactRepository _contactRepository, ILeadRepository _leadRepository, INotificationService _notificationService, ApplicationDbContext _context, IWebHostEnvironment _env, ICallStatusRepository _callStatusRepository, IProvinceService _provinceService, ISourceService _sourceService, IDistrictService _districtService, ILogService _logService, UserManager<ApplicationUser> _userManager, IHCAService _hcaService) : IContactService
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
            TelesalesManagerName = telesalesManager.Name,
            contact.SourceId,
            user.ManagerId,
            contact.BranchId
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
        data.BranchId = args.BranchId;
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
            if (!await _context.Branches.AnyAsync(x => x.Id == args.BranchId)) return TResult.Failed("Chi nhánh không tồn tại");
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
                SourceId = args.SourceId,
                BranchId = args.BranchId
            };
            if (_hcaService.IsUserInRole(RoleName.Telesales))
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
        var lead = await _leadRepository.FindAsync(args.LeadId);
        if (lead is null) return TResult.Failed("Không tìm thấy liên hệ!");
        lead.Confirm2 = args.Confirm2;
        lead.Note = args.Reason;
        await _logService.AddAsync($"Cập nhật xác nhận 2 cho liên hệ {lead.Name} - {lead.PhoneNumber} thành {EnumHelper.GetDisplayName(args.Confirm2)}");
        await _leadRepository.UpdateAsync(lead);
        return TResult.Success;
    }

    public async Task<TResult> UpdateAttendanceScheduleAsync(UpdateAttendanceScheduleArgs args)
    {
        var lead = await _leadRepository.FindAsync(args.LeadId);
        if (lead is null) return TResult.Failed("Không tìm thấy lịch hẹn!");
        var eventExists = await _context.Events.AnyAsync(e => e.Id == args.EventId);
        if (!eventExists) return TResult.Failed("Sự kiện không tồn tại!");
        if (lead.Status == LeadStatus.CloseDeal) return TResult.Failed($"Không thể thay đổi lịch hẹn của lead đã ở trạng thái {EnumHelper.GetDisplayName(LeadStatus.CloseDeal)}!");
        if (lead.Status == LeadStatus.Checkin) return TResult.Failed($"Không thể thay đổi lịch hẹn của lead đã ở trạng thái {EnumHelper.GetDisplayName(LeadStatus.Checkin)}!");
        await _context.LeadHistories.AddAsync(new LeadHistory
        {
            LeadId = lead.Id,
            AttendanceId = lead.AttendanceId,
            EventId = lead.EventId,
            CreatedBy = _hcaService.GetUserId(),
            EventDate = lead.EventDate,
            Note = lead.Note,
            SalesId = lead.SalesId,
            TelesaleId = lead.TelesaleId,
            ToById = lead.ToById
        });
        lead.Name = args.Name;
        lead.EventId = args.EventId;
        if (!string.IsNullOrWhiteSpace(args.Note))
        {
            lead.Note = args.Note;
        }
        lead.Confirm2 = args.Confirm2;
        lead.AppointmentDate = DateTime.Now;
        if (lead.EventDate.Date != args.EventDate.Date)
        {
            lead.Status = LeadStatus.Pending;
            var ems = await (from u in _context.Users
                            join ur in _context.UserRoles on u.Id equals ur.UserId
                            join r in _context.Roles on ur.RoleId equals r.Id
                            where r.Name == RoleName.EM && u.Status == UserStatus.Working
                            select u.Id).ToListAsync();
            foreach (var em in ems)
            {
                await _notificationService.CreateAsync("Lịch hẹn bị thay đổi", $"Lịch hẹn của {lead.Name} - {lead.PhoneNumber} đã bị thay đổi từ {lead.EventDate:dd/MM/yyyy} sang ngày {args.EventDate:dd/MM/yyyy}.", em);
            }
        }
        lead.EventDate = args.EventDate;
        await _logService.AddAsync($"Cập nhật lịch hẹn cho {lead.PhoneNumber} - {lead.Name}: ngày {lead.EventDate:dd/MM/yyyy}");
        await _leadRepository.UpdateAsync(lead);
        return TResult.Success;
    }

    public Task<ListResult<object>> GetAttendanceScheduleListAsync(ContactFilterOptions filterOptions) => _contactRepository.GetAttendanceScheduleListAsync(filterOptions);

    public Task<TResult<object>> GetTmrReportAsync() => _contactRepository.GetTmrReportAsync();

    public Task<ListResult<object>> DialedCallsAsync(ContactFilterOptions filterOptions) => _contactRepository.DialedCallsAsync(filterOptions);

    public async Task<ListResult<dynamic>> TransferSourceListAsync(ContactTransferFilterOptions filterOptions)
    {
        var query = from c in _context.Contacts
                    join s in _context.Sources on c.SourceId equals s.Id into cs
                    from s in cs.DefaultIfEmpty()
                    join t in _context.Teams on s.TeamId equals t.Id into st
                    from t in st.DefaultIfEmpty()
                    join u in _context.Users on c.UserId equals u.Id into cu
                    from u in cu.DefaultIfEmpty()
                    where c.Status != ContactStatus.Blacklisted
                    select new ContactTransferListItem
                    {
                        Id = c.Id,
                        Name = c.Name,
                        PhoneNumber = c.PhoneNumber,
                        CreatedDate = c.CreatedDate,
                        SourceId = c.SourceId,
                        SourceName = s != null ? s.Name : null,
                        GroupId = s != null ? s.TeamId : null,
                        GroupName = t != null ? t.Name : null,
                        TelesalesId = c.UserId,
                        TelesalesName = u != null ? u.Name : null
                    };

        if (filterOptions.GroupId.HasValue)
        {
            query = query.Where(x => x.GroupId == filterOptions.GroupId);
        }
        if (filterOptions.SourceId.HasValue)
        {
            query = query.Where(x => x.SourceId == filterOptions.SourceId);
        }
        if (filterOptions.TelesalesId.HasValue)
        {
            query = query.Where(x => x.TelesalesId == filterOptions.TelesalesId);
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.Contains(filterOptions.PhoneNumber));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            var keyword = filterOptions.Name.ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(keyword));
        }

        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<dynamic>.Success(query, filterOptions);
    }

    public async Task<TResult> TransferSourceBySearchAsync(ContactTransferBySearchArgs args)
    {
        var validatePermission = ValidateTransferPermission();
        if (!validatePermission.Succeeded) return validatePermission;
        var validateDestination = await ValidateTransferDestinationAsync(args.Destination);
        if (!validateDestination.Succeeded) return validateDestination;

        var contacts = await BuildTransferQuery(args.Filter).ToListAsync();
        return await ApplyTransferAsync(contacts, args.Destination, "search");
    }

    public async Task<TResult> TransferSourceByCaseAsync(ContactTransferByCaseArgs args)
    {
        var validatePermission = ValidateTransferPermission();
        if (!validatePermission.Succeeded) return validatePermission;
        if (args.ContactIds.Count == 0) return TResult.Failed("Vui lòng chọn contact cần chuyển!");
        var validateDestination = await ValidateTransferDestinationAsync(args.Destination);
        if (!validateDestination.Succeeded) return validateDestination;

        var contacts = await _context.Contacts
            .Where(x => x.Status != ContactStatus.Blacklisted)
            .Where(x => args.ContactIds.Contains(x.Id))
            .ToListAsync();

        return await ApplyTransferAsync(contacts, args.Destination, "case");
    }

    public async Task<TResult> TransferSourceByFileAsync(ContactTransferByFileArgs args)
    {
        var validatePermission = ValidateTransferPermission();
        if (!validatePermission.Succeeded) return validatePermission;
        if (args.File is null || args.File.Length == 0) return TResult.Failed("Vui lòng chọn file Excel hợp lệ!");
        if (!Path.GetExtension(args.File.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            return TResult.Failed("Chỉ hỗ trợ file định dạng .xlsx");
        }

        var destination = new ContactTransferDestinationArgs
        {
            GroupId = args.GroupId,
            SourceId = args.SourceId,
            TeamId = args.TeamId,
            TelesalesId = args.TelesalesId
        };
        var validateDestination = await ValidateTransferDestinationAsync(destination);
        if (!validateDestination.Succeeded) return validateDestination;

        using var package = new ExcelPackage(args.File.OpenReadStream());
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
        if (worksheet?.Dimension is null) return TResult.Failed("File không có dữ liệu!");

        var uploadRows = new List<(string Name, string PhoneNumber)>();
        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
        {
            var name = worksheet.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty;
            var phoneNumber = worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phoneNumber)) continue;
            uploadRows.Add((name, phoneNumber));
        }

        if (uploadRows.Count == 0) return TResult.Failed("Không tìm thấy dữ liệu hợp lệ trong file!");

        var phoneNumbers = uploadRows.Select(x => x.PhoneNumber).Distinct().ToList();
        var candidates = await _context.Contacts
            .Where(x => x.Status != ContactStatus.Blacklisted)
            .Where(x => !string.IsNullOrEmpty(x.PhoneNumber) && phoneNumbers.Contains(x.PhoneNumber))
            .ToListAsync();

        var contacts = candidates
            .Where(c => uploadRows.Any(x => x.PhoneNumber == c.PhoneNumber && x.Name.Equals(c.Name, StringComparison.CurrentCultureIgnoreCase)))
            .GroupBy(x => x.Id)
            .Select(x => x.First())
            .ToList();

        return await ApplyTransferAsync(contacts, destination, "file");
    }

    public async Task<ListResult<dynamic>> RevokeSourceListAsync(ContactRevokeSourceFilterOptions filterOptions)
    {
        var validatePermission = ValidateRevokePermission();
        if (!validatePermission.Succeeded) return ListResult<dynamic>.Failed(validatePermission.Message ?? "Bạn không có quyền thực hiện chức năng thu hồi nguồn!");

        var query = from c in _context.Contacts
                    join s in _context.Sources on c.SourceId equals s.Id into cs
                    from s in cs.DefaultIfEmpty()
                    join g in _context.Teams on s.TeamId equals g.Id into sg
                    from g in sg.DefaultIfEmpty()
                    join u in _context.Users on c.UserId equals u.Id
                    join t in _context.Teams on u.TeamId equals t.Id into ut
                    from t in ut.DefaultIfEmpty()
                    where c.Status != ContactStatus.Blacklisted
                    where !_context.CallHistories.Any(x => x.ContactId == c.Id)
                    select new ContactRevokeListItem
                    {
                        Id = c.Id,
                        Name = c.Name,
                        PhoneNumber = c.PhoneNumber,
                        CreatedDate = c.CreatedDate,
                        SourceId = c.SourceId,
                        SourceName = s != null ? s.Name : null,
                        GroupId = s != null ? s.TeamId : null,
                        GroupName = g != null ? g.Name : null,
                        TeamId = u.TeamId,
                        TeamName = t != null ? t.Name : null,
                        TelesalesId = c.UserId,
                        TelesalesName = u.Name
                    };

        if (filterOptions.GroupId.HasValue)
        {
            query = query.Where(x => x.GroupId == filterOptions.GroupId);
        }
        if (filterOptions.TeamId.HasValue)
        {
            query = query.Where(x => x.TeamId == filterOptions.TeamId);
        }
        if (filterOptions.SourceId.HasValue)
        {
            query = query.Where(x => x.SourceId == filterOptions.SourceId);
        }
        if (filterOptions.TelesalesId.HasValue)
        {
            query = query.Where(x => x.TelesalesId == filterOptions.TelesalesId);
        }

        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<dynamic>.Success(query, filterOptions);
    }

    public async Task<TResult> RevokeSourceByCaseAsync(ContactRevokeSourceByCaseArgs args)
    {
        var validatePermission = ValidateRevokePermission();
        if (!validatePermission.Succeeded) return validatePermission;
        if (args.ContactIds.Count == 0) return TResult.Failed("Vui lòng chọn contact cần thu hồi!");

        var contacts = await _context.Contacts
            .Where(x => x.Status != ContactStatus.Blacklisted)
            .Where(x => x.UserId != null)
            .Where(x => args.ContactIds.Contains(x.Id))
            .Where(x => !_context.CallHistories.Any(ch => ch.ContactId == x.Id))
            .ToListAsync();

        if (!contacts.Any()) return TResult.Failed("Không có contact phù hợp để thu hồi!");

        foreach (var contact in contacts)
        {
            contact.UserId = null;
            contact.ModifiedDate = DateTime.Now;
            contact.ModifiedBy = _hcaService.GetUserId();
        }

        _context.Contacts.UpdateRange(contacts);
        await _context.SaveChangesAsync();

        await _logService.AddAsync($"Thu hồi nguồn {contacts.Count} contact chưa gọi.");
        return TResult.Ok(new
        {
            revokedCount = contacts.Count
        });
    }

    public Task<TResult> GetReportDataSourceAsync(ReportDataSourceFilterOptions filterOptions) => _contactRepository.GetReportDataSourceAsync(filterOptions);

    public Task<TResult<byte[]?>> ExportReportDataSourceAsync(ReportDataSourceFilterOptions filterOptions) => _contactRepository.ExportReportDataSourceAsync(filterOptions);

    public Task<TResult> GetTmrDataReportAsync(TmrDataReportFilterOptions filterOptions) => _contactRepository.GetTmrDataReportAsync(filterOptions);

    public Task<TResult<byte[]?>> ExportTmrDataReportAsync(TmrDataReportFilterOptions filterOptions) => _contactRepository.ExportTmrDataReportAsync(filterOptions);

    public Task<TResult<byte[]?>> ExportMultipleAssignReportAsync(MultipleAssignReportFilterOptions filterOptions) => _contactRepository.ExportMultipleAssignReportAsync(filterOptions);

    public Task<TResult> GetMultipleAssignReportAsync(MultipleAssignReportFilterOptions filterOptions) => _contactRepository.GetMultipleAssignReportAsync(filterOptions);

    private IQueryable<Contact> BuildTransferQuery(ContactTransferFilterOptions filter)
    {
        var query = _context.Contacts.Where(x => x.Status != ContactStatus.Blacklisted);
        if (filter.GroupId.HasValue)
        {
            query = query.Where(x => x.SourceId != null && _context.Sources.Any(s => s.Id == x.SourceId && s.TeamId == filter.GroupId));
        }
        if (filter.SourceId.HasValue)
        {
            query = query.Where(x => x.SourceId == filter.SourceId);
        }
        if (filter.TelesalesId.HasValue)
        {
            query = query.Where(x => x.UserId == filter.TelesalesId);
        }
        if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.Contains(filter.PhoneNumber));
        }
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var keyword = filter.Name.ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(keyword));
        }
        return query;
    }

    private TResult ValidateTransferPermission()
    {
        if (!_hcaService.IsUserInAnyRole(RoleName.Admin, RoleName.AdminData, RoleName.Dot))
        {
            return TResult.Failed("Bạn không có quyền thực hiện chức năng chuyển nguồn!");
        }
        return TResult.Success;
    }

    private TResult ValidateRevokePermission()
    {
        if (!_hcaService.IsUserInAnyRole(RoleName.Admin, RoleName.AdminData, RoleName.Dot))
        {
            return TResult.Failed("Bạn không có quyền thực hiện chức năng thu hồi nguồn!");
        }
        return TResult.Success;
    }

    private async Task<TResult> ValidateTransferDestinationAsync(ContactTransferDestinationArgs destination)
    {
        if (!destination.SourceId.HasValue) return TResult.Failed("Vui lòng chọn nguồn đích!");

        var source = await _context.Sources.AsNoTracking().FirstOrDefaultAsync(x => x.Id == destination.SourceId.Value);
        if (source is null) return TResult.Failed("Nguồn đích không tồn tại!");

        if (destination.GroupId.HasValue && source.TeamId != destination.GroupId.Value)
        {
            return TResult.Failed("Nguồn đích không thuộc group đã chọn!");
        }

        if (destination.TeamId.HasValue)
        {
            var teamExists = await _context.Teams.AnyAsync(x => x.Id == destination.TeamId.Value);
            if (!teamExists) return TResult.Failed("Team đích không tồn tại!");
        }

        if (destination.TelesalesId.HasValue)
        {
            var telesales = await _userManager.FindByIdAsync(destination.TelesalesId.Value.ToString());
            if (telesales is null) return TResult.Failed("Telesales đích không tồn tại!");
            if (!await _userManager.IsInRoleAsync(telesales, RoleName.Telesales)) return TResult.Failed("Người dùng đích không phải telesales!");
            if (destination.TeamId.HasValue && telesales.TeamId != destination.TeamId)
            {
                return TResult.Failed("Telesales đích không thuộc team đã chọn!");
            }
        }

        return TResult.Success;
    }

    private async Task<TResult> ApplyTransferAsync(List<Contact> contacts, ContactTransferDestinationArgs destination, string transferType)
    {
        if (!contacts.Any()) return TResult.Failed("Không có contact nào phù hợp để chuyển!");

        foreach (var contact in contacts)
        {
            contact.SourceId = destination.SourceId;
            if (destination.TelesalesId.HasValue)
            {
                contact.UserId = destination.TelesalesId;
            }
            else if (destination.TeamId.HasValue)
            {
                contact.UserId = null;
            }
            contact.ModifiedDate = DateTime.Now;
            contact.ModifiedBy = _hcaService.GetUserId();
        }

        _context.Contacts.UpdateRange(contacts);
        await _context.SaveChangesAsync();

        await _logService.AddAsync($"Chuyển nguồn {contacts.Count} contact theo hình thức {transferType}. Destination SourceId={destination.SourceId}, TeamId={destination.TeamId}, TelesalesId={destination.TelesalesId}");
        return TResult.Ok(new
        {
            transferredCount = contacts.Count
        });
    }

    public async Task<TResult> GetDetailByPhoneAsync(string phone)
    {
        var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.PhoneNumber == phone);
        if (contact is null) return TResult.Failed("Contact not found");
        var callHistories = await _context.CallHistories.Where(x => x.ContactId == contact.Id)
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new
            {
                x.Id,
                x.Note,
                x.CallStatusId,
                x.ContactId,
                x.Age,
                x.ExtraStatus,
                x.CreatedDate,
                Caller = _context.Users.Where(u => u.Id == x.CreatedBy).Select(u => u.Name).FirstOrDefault(),
                Status = _context.CallStatuses.Where(cs => cs.Id == x.CallStatusId).Select(cs => cs.Name).FirstOrDefault()
            })
            .ToListAsync();
        return TResult.Ok(new
        {
            contact.Id,
            contact.Name,
            contact.PhoneNumber,
            contact.Email,
            contact.Address,
            contact.Confirm1,
            contact.Note,
            contact.CreatedDate,
            CallHistories = callHistories
        });
    }

    private class ContactTransferListItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? SourceId { get; set; }
        public string? SourceName { get; set; }
        public int? GroupId { get; set; }
        public string? GroupName { get; set; }
        public Guid? TelesalesId { get; set; }
        public string? TelesalesName { get; set; }
    }

    private class ContactRevokeListItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? SourceId { get; set; }
        public string? SourceName { get; set; }
        public int? GroupId { get; set; }
        public string? GroupName { get; set; }
        public int? TeamId { get; set; }
        public string? TeamName { get; set; }
        public Guid? TelesalesId { get; set; }
        public string? TelesalesName { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using Waffle.Core.Constants;
using Waffle.Core.Interfaces.IRepository.Leads;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Interfaces.IService.Events;
using Waffle.Core.Services.Leads.Args;
using Waffle.Core.Services.Leads.Filters;
using Waffle.Entities;
using Waffle.Entities.Contracts;
using Waffle.Models;

namespace Waffle.Core.Services.Leads;

public class LeadService(ILeadRepository _leadRepository, IVoucherService _voucherService, IHCAService _hcaService, ILogService _logService, UserManager<ApplicationUser> _userManager) : ILeadService
{
    public async Task<TResult> AddAsync(LeadCreateArgs args)
    {
        if (_hcaService.IsUserInRole(RoleName.Admin)) return TResult.Failed("Bạn không có quyền tạo Key-In");
        var currentUserId = _hcaService.GetUserId();
        var lead = new Lead
        {
            Id = Guid.NewGuid(),
            CreatedBy = currentUserId,
            Email = args.Email,
            Name = args.Name,
            CreatedDate = DateTime.Now,
            EventId = args.EventId,
            EventDate = args.EventDate,
            Note = args.Note,
            PhoneNumber = args.PhoneNumber,
            BranchId = args.BranchId,
            TelesaleId = args.TelesalesId,
            Gender = args.Gender
        };
        if (_hcaService.IsUserInAnyRole(RoleName.Sales, RoleName.Event))
        {
            lead.SourceId = SourceConstant.PRIVATE;
        }
        if (_hcaService.IsUserInRole(RoleName.Telesale))
        {
            lead.SourceId = SourceConstant.TELE_OPC;
        }
        if (_hcaService.IsUserInAnyRole(RoleName.SalesManager, RoleName.TelesaleManager))
        {
            var currentUser = await _userManager.FindByIdAsync(currentUserId.ToString());
            if (currentUser == null) return TResult.Failed("Người dùng không tồn tại!");
            if (_hcaService.IsUserInRole(RoleName.TelesaleManager))
            {
                var manager = await _userManager.FindByIdAsync(currentUser.TmId.GetValueOrDefault().ToString());
                if (manager == null) return TResult.Failed("Người dùng không tồn tại!");
                if (manager.SourceId == null) return TResult.Failed("Nguồn của Telesale Manager chưa được thiết lập!");
                lead.SourceId = manager.SourceId;
            }
            if (_hcaService.IsUserInRole(RoleName.SalesManager))
            {
                var manager = await _userManager.FindByIdAsync(currentUser.SmId.GetValueOrDefault().ToString());
                if (manager == null) return TResult.Failed("Người dùng không tồn tại!");
                if (manager.SourceId == null) return TResult.Failed("Nguồn của Telesale Manager chưa được thiết lập!");
                lead.SourceId = manager.SourceId;
            }
            lead.Status = LeadStatus.Approved;
        }
        if (_hcaService.IsUserInAnyRole(RoleName.Dos, RoleName.Dot))
        {
            var currentUser = await _userManager.FindByIdAsync(currentUserId.ToString());
            if (currentUser == null) return TResult.Failed("Người dùng không tồn tại!");
            if (currentUser.SourceId == null) return TResult.Failed("Nguồn của Dos/ Dot chưa được thiết lập!");
            lead.SourceId = currentUser.SourceId;
            lead.Status = LeadStatus.Approved;
        }
        if (args.SubLeads != null && args.SubLeads.Count > 0)
        {
            await _leadRepository.AddSubLeadsAsync(lead.Id, args.SubLeads);
        }
        await _logService.AddAsync($"Tạo mới khách hàng {lead.Name} - {lead.PhoneNumber}");
        await _leadRepository.AddAsync(lead);
        return TResult.Success;
    }

    public async Task<TResult> AllowedDuplicateAsync(Guid id)
    {
        var lead = await _leadRepository.FindAsync(id);
        if (lead == null) return TResult.Failed("Không tìm thấy khách hàng!");
        lead.Duplicated = !lead.Duplicated;
        await _logService.AddAsync(lead.Duplicated ? "Cho phép trùng số ĐDCN" : "Không cho phép trùng số ĐDCN");
        await _leadRepository.UpdateAsync(lead);
        return TResult.Success;
    }

    public Task<IEnumerable<string?>> AllPhoneNumbersAsync() => _leadRepository.AllPhoneNumbersAsync();

    public async Task<TResult> CheckinAsync(LeadCheckinArgs args)
    {
        var lead = await _leadRepository.FindAsync(args.LeadId);
        if (lead == null) return TResult.Failed("Không tìm thấy khách hàng!");
        if (string.IsNullOrWhiteSpace(args.IdentityNumber)) return TResult.Failed("Chưa nhập số ĐDCN!");

        var existingLead = await _leadRepository.FindByIdentityNumberAsync(args.IdentityNumber);
        if (existingLead != null && existingLead.Id != lead.Id && !existingLead.Dupplicated) return TResult.Failed($"Khách hàng đã tham dự sự kiện {existingLead.EventName} ngày {existingLead.EventDate:dd/MM/yyyy}");

        lead.Status = LeadStatus.Checkin;
        lead.Note = args.Note;
        lead.IdentityNumber = args.IdentityNumber;
        lead.DateOfBirth = args.DateOfBirth;
        lead.Gender = args.Gender;
        lead.Name = args.Name;
        lead.AttendanceId = args.AttendanceId;

        await _logService.AddAsync($"Khách hàng check-in. Số ĐDCN: {args.IdentityNumber}.");
        await _leadRepository.SaveFeedbackAsync(lead.Id, args.TableId);
        if (args.SubLeads != null && args.SubLeads.Count > 0)
        {
            await _leadRepository.UpdateSubLeadsAsync(lead.Id, args.SubLeads);
        }
        await _leadRepository.UpdateAsync(lead);
        if (existingLead != null)
        {
            var duplicatedLead = await _leadRepository.FindAsync(existingLead.Id);
            if (duplicatedLead == null) return TResult.Failed("Không tìm thấy khách hàng!");
            duplicatedLead.Duplicated = false;
            await _leadRepository.UpdateAsync(duplicatedLead);
        }
        return TResult.Success;
    }

    public async Task<TResult<object>> DetailAsync(Guid id)
    {
        var lead = await _leadRepository.FindAsync(id);
        if (lead == null) return TResult<object>.Failed("Không tìm thấy khách hàng!");
        var sales = new ApplicationUser();
        if (lead.SalesId.HasValue)
        {
            sales = await _userManager.FindByIdAsync(lead.SalesId.GetValueOrDefault().ToString());
            if (sales == null) return TResult<object>.Failed("Không tìm thấy nhân viên kinh doanh!");
        }
        var feedback = await _leadRepository.GetFeedbackAsync(lead.Id);
        var creator = await _userManager.FindByIdAsync(lead.CreatedBy.ToString());
        if (creator == null) return TResult<object>.Failed("Không tìm thấy người tạo khách hàng!");
        return TResult<object>.Ok(new
        {
            lead.Id,
            lead.Name,
            lead.PhoneNumber,
            lead.Email,
            lead.EventId,
            lead.EventDate,
            lead.Note,
            lead.Status,
            lead.CreatedDate,
            lead.CreatedBy,
            lead.IdentityNumber,
            lead.DateOfBirth,
            lead.Address,
            lead.SalesId,
            lead.TelesaleId,
            lead.Gender,
            lead.BranchId,
            lead.SourceId,
            lead.AttendanceId,
            lead.ToById,
            SalesManagerId = sales.SmId,
            sales.DosId,
            feedback?.TableId,
            SubLeads = await _leadRepository.GetSubLeadsAsync(lead.Id),
            CreatorLeaderId = creator.SmId,
            lead.Voucher1Id,
            lead.Voucher2Id
        });
    }

    public async Task<TResult<byte[]?>> ExportCheckinAsync(LeadCheckinListFilterOptions filterOptions)
    {
        var data = await _leadRepository.GetExportCheckinDataAsync(filterOptions);
        if (data.Count == 0) return TResult<byte[]?>.Failed("Không có dữ liệu để xuất!");

        using var pgk = new ExcelPackage();
        var worksheet = pgk.Workbook.Worksheets.Add("Leads");
        worksheet.Cells[1, 1].Value = "STT";
        worksheet.Cells[1, 2].Value = "Họ và tên";
        worksheet.Cells[1, 3].Value = "Số ĐT";
        worksheet.Cells[1, 4].Value = "CCCD";
        worksheet.Cells[1, 5].Value = "Ngày sinh";
        worksheet.Cells[1, 6].Value = "Ngày tham dự";
        worksheet.Cells[1, 7].Value = "Khung giờ";
        worksheet.Cells[1, 8].Value = "Số hợp đồng";
        worksheet.Cells[1, 9].Value = "GTHD";
        worksheet.Cells[1, 10].Value = "Đã thanh toán";
        worksheet.Cells[1, 11].Value = "Người Key-in";
        worksheet.Cells[1, 12].Value = "Team Key-in";
        worksheet.Cells[1, 13].Value = "DOS";
        worksheet.Cells[1, 14].Value = "Số bàn";
        worksheet.Cells[1, 15].Value = "Sales Manager";
        worksheet.Cells[1, 16].Value = "Rep";
        worksheet.Cells[1, 17].Value = "T.O";
        worksheet.Cells[1, 18].Value = "Nguồn";
        worksheet.Cells[1, 19].Value = "Trạng thái tham dự";

        var row = 2;
        foreach (var item in data)
        {
            worksheet.Cells[row, 1].Value = row - 1;
            worksheet.Cells[row, 2].Value = item.CustomerName;
            worksheet.Cells[row, 3].Value = item.CustomerPhoneNumber;
            worksheet.Cells[row, 4].Value = item.CustomerIdNumber;
            worksheet.Cells[row, 5].Value = item.DateOfBirth?.ToString("dd/MM/yyyy");
            worksheet.Cells[row, 6].Value = item.EventDate.ToString("dd/MM/yyyy");
            worksheet.Cells[row, 7].Value = item.EventName;
            worksheet.Cells[row, 8].Value = item.ContractCode;
            worksheet.Cells[row, 9].Value = item.TotalAmount;
            worksheet.Cells[row, 9].Style.Numberformat.Format = "#,##0";
            worksheet.Cells[row, 10].Value = item.AmountPaid;
            worksheet.Cells[row, 10].Style.Numberformat.Format = "#,##0";
            worksheet.Cells[row, 11].Value = item.KeyInName;
            worksheet.Cells[row, 12].Value = item.TeamKeyIn;
            worksheet.Cells[row, 13].Value = item.DOS;
            worksheet.Cells[row, 14].Value = item.TableName;
            worksheet.Cells[row, 15].Value = item.SalesManagerName;
            worksheet.Cells[row, 16].Value = item.SalesName;
            worksheet.Cells[row, 17].Value = item.ToName;
            worksheet.Cells[row, 18].Value = item.SourceName;
            worksheet.Cells[row, 19].Value = item.AttendanceName;
            row++;
        }

        worksheet.Row(1).Style.Font.Bold = true;
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var cells = worksheet.Cells[1, 1, row - 1, 19];
        cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        return TResult<byte[]?>.Ok(await pgk.GetAsByteArrayAsync());

    }

    public Task<Lead?> FindAsync(Guid id) => _leadRepository.FindAsync(id);

    public Task<Lead?> FindByPhoneNumberAsync(string? phoneNumber) => _leadRepository.FindByPhoneNumberAsync(phoneNumber);

    public Task<ListResult<object>> GetCheckinListAsync(LeadCheckinListFilterOptions filterOptions) => _leadRepository.GetCheckinListAsync(filterOptions);

    public Task<ListResult<object>> GetWaitingListAsync(LeadWaittingListFilterOptions filterOptions) => _leadRepository.GetWaitingListAsync(filterOptions);

    public async Task<TResult> RejectAsync(LeadRejectArgs args)
    {
        var lead = await _leadRepository.FindAsync(args.Id);
        if (lead == null) return TResult.Failed("Không tìm thấy khách hàng!");
        await _logService.AddAsync($"Khách hàng từ chối. Lý do: {args.Note}");
        lead.Status = LeadStatus.LeadReject;
        lead.Note = args.Note;
        await _leadRepository.UpdateAsync(lead);
        return TResult.Success;
    }

    public async Task<TResult> UpdateAsync(LeadUpdateArgs args)
    {
        var lead = await _leadRepository.FindAsync(args.Id);
        if (lead == null) return TResult.Failed("Không tìm thấy khách hàng!");
        var creator = await _userManager.FindByIdAsync(lead.CreatedBy.ToString());
        if (creator is null) return TResult.Failed("Không tìm thấy người tạo khách hàng!");
        if (string.IsNullOrWhiteSpace(args.IdentityNumber)) return TResult.Failed("Chưa nhập số CCCD!");
        lead.Name = args.Name;
        lead.DateOfBirth = args.DateOfBirth;
        lead.Gender = args.Gender;
        lead.PhoneNumber = args.PhoneNumber;
        lead.Email = args.Email;
        lead.BranchId = args.BranchId;
        lead.Address = args.Address;
        lead.EventDate = args.EventDate;
        lead.EventId = args.EventId;
        lead.Note = args.Note;
        lead.CreatedBy = args.CreatedBy;
        lead.IdentityNumber = args.IdentityNumber;
        if (args.SubLeads != null && args.SubLeads.Count > 0)
        {
            await _leadRepository.UpdateSubLeadsAsync(lead.Id, args.SubLeads);
        }
        await _logService.AddAsync($"Cập nhật thông tin khách hàng {lead.Name} - {lead.PhoneNumber}");
        await _leadRepository.UpdateAsync(lead);
        return TResult.Success;
    }

    public async Task<TResult> UpdateFeedbackAsync(LeadUpdateFeedbackArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.IdentityNumber)) return TResult.Failed("Chưa nhập số ĐDCN!");
        var lead = await _leadRepository.FindAsync(args.Id);
        if (lead is null) return TResult.Failed("Không tìm thấy khách hàng!");
        var feedback = await _leadRepository.GetFeedbackAsync(lead.Id);
        if (feedback is null)
        {
            feedback = new LeadFeedback
            {
                LeadId = lead.Id
            };
            await _leadRepository.AddFeedbackAsync(feedback);
        }
        var creator = await _userManager.FindByIdAsync(lead.CreatedBy.ToString());
        if (creator is null) return TResult.Failed("Không tìm thấy người tạo khách hàng!");
        if (args.Voucher1Id.HasValue)
        {
            var voucher1 = await _voucherService.FindAsync(args.Voucher1Id.GetValueOrDefault());
            if (voucher1 is null) return TResult.Failed("Voucher không tồn tại");
            if (await _voucherService.IsUsedAsync(voucher1.Id, lead.Id)) return TResult.Failed("Voucher đã được sử dụng");
            voucher1.ActiveAt = DateTime.Now;
            voucher1.ExpiredDate = DateTime.Now.AddDays(voucher1.ExpiredDays);
            voucher1.Status = VoucherStatus.Redeemed;
        }
        if (args.Voucher2Id.HasValue)
        {
            var voucher2 = await _voucherService.FindAsync(args.Voucher2Id.GetValueOrDefault());
            if (voucher2 is null) return TResult.Failed("Voucher không tồn tại");
            if (await _voucherService.IsUsedAsync(voucher2.Id, lead.Id)) return TResult.Failed("Voucher đã được sử dụng");
            voucher2.ActiveAt = DateTime.Now;
            voucher2.ExpiredDate = DateTime.Now.AddDays(voucher2.ExpiredDays);
            voucher2.Status = VoucherStatus.Redeemed;
        }
        if (lead.Voucher1Id.HasValue)
        {
            var oldVoucher1 = await _voucherService.FindAsync(lead.Voucher1Id.GetValueOrDefault());
            if (oldVoucher1 != null)
            {
                oldVoucher1.Status = VoucherStatus.Unused;
                oldVoucher1.ActiveAt = null;
                oldVoucher1.ExpiredDate = null;
            }
        }
        if (lead.Voucher2Id.HasValue)
        {
            var oldVoucher2 = await _voucherService.FindAsync(lead.Voucher2Id.GetValueOrDefault());
            if (oldVoucher2 != null)
            {
                oldVoucher2.Status = VoucherStatus.Unused;
                oldVoucher2.ActiveAt = null;
                oldVoucher2.ExpiredDate = null;
            }
        }
        lead.IdentityNumber = args.IdentityNumber;
        lead.ToById = args.ToById;
        lead.DateOfBirth = args.DateOfBirth;
        lead.Address = args.Address;
        lead.SalesId = args.SalesId;
        lead.Gender = args.Gender;
        lead.Name = args.Name;
        lead.Note = args.Note;
        lead.PhoneNumber = args.PhoneNumber;
        lead.Email = args.Email;
        lead.AttendanceId = args.AttendanceId;
        lead.Voucher1Id = args.Voucher1Id;
        lead.Voucher2Id = args.Voucher2Id;
        lead.CreatedBy = args.CreatedBy;
        feedback.TransportId = args.TransportId;
        feedback.TableId = args.TableId;
        feedback.InterestLevel = args.InterestLevel;
        feedback.FinancialSituation = args.FinancialSituation;
        feedback.JobKindId = args.JobKindId;
        await _leadRepository.UpdateFeedbackAsync(lead, feedback);
        return TResult.Success;
    }
}

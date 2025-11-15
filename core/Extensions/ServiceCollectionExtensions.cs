using Waffle.Core.Foundations;
using Waffle.Core.Interfaces;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IRepository.Calls;
using Waffle.Core.Interfaces.IRepository.Events;
using Waffle.Core.Interfaces.IRepository.Finances;
using Waffle.Core.Interfaces.IRepository.Leads;
using Waffle.Core.Interfaces.IRepository.Settings;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Interfaces.IService.Calls;
using Waffle.Core.Interfaces.IService.Events;
using Waffle.Core.Interfaces.IService.Finances;
using Waffle.Core.Senders;
using Waffle.Core.Services;
using Waffle.Core.Services.Attendances;
using Waffle.Core.Services.Branches;
using Waffle.Core.Services.Calls;
using Waffle.Core.Services.Cards;
using Waffle.Core.Services.Contacts;
using Waffle.Core.Services.Contracts;
using Waffle.Core.Services.Departments;
using Waffle.Core.Services.Districts;
using Waffle.Core.Services.Ecommerces;
using Waffle.Core.Services.Events;
using Waffle.Core.Services.Finances.Bills;
using Waffle.Core.Services.Finances.Counpons;
using Waffle.Core.Services.Finances.Interfaces;
using Waffle.Core.Services.Finances.Invoices;
using Waffle.Core.Services.Histories;
using Waffle.Core.Services.JobKinds;
using Waffle.Core.Services.Leads;
using Waffle.Core.Services.Provinces;
using Waffle.Core.Services.Rooms;
using Waffle.Core.Services.Tables;
using Waffle.Core.Services.Teams;
using Waffle.Core.Services.Teams.Interfaces;
using Waffle.Core.Services.Vouchers;
using Waffle.Data.ContentGenerators;
using Waffle.Infrastructure.Repositories;
using Waffle.Infrastructure.Repositories.Calls;
using Waffle.Infrastructure.Repositories.Events;
using Waffle.Infrastructure.Repositories.Leads;
using Waffle.Infrastructure.Repositories.Payments;
using Waffle.Infrastructure.Repositories.Settings;
using Waffle.Infrastructure.Repositories.Teams;
using Waffle.Infrastructure.Repositories.Tele;

namespace Waffle.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddScoped<ISettingService, SettingService>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<ICatalogRepository, CatalogRepository>();
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IComponentRepository, ComponentRepository>();
        services.AddScoped<IComponentService, ComponentService>();
        services.AddScoped<IFileService, FileExplorerService>();
        services.AddScoped<ILocalizationService, LocalizationService>();
        services.AddScoped<ILocalizationRepository, LocalizationRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWorkService, WorkService>();
        services.AddScoped<IWorkContentRepository, WorkItemRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderSerivce>();
        services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISettingRepository, SettingRepository>();
        services.AddScoped<IMigrationService, MigrationService>();
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<ITableRepository, TableRepository>();
        services.AddScoped<ITableService, TableService>();
        services.AddScoped<ICallHistoryRepository, CallHistoryRepository>();
        services.AddScoped<ICallHistoryService, CallHistoryService>();
        services.AddScoped<ICampaignRepository, CampaignRepository>();
        services.AddScoped<ICampaignService, CampaignService>();
        services.AddScoped<ILeadRepository, LeadRepository>();
        services.AddScoped<ILeadService, LeadService>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<ILoanService, LoanService>();

        services.AddScoped<IProvinceRepository, ProvinceRepository>();
        services.AddScoped<IProvinceService, ProvinceService>();
        services.AddScoped<IDistrictRepository, DistrictRepository>();
        services.AddScoped<IDistrictService, DistrictService>();

        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<ITransportRepository, TransportRepository>();
        services.AddScoped<ITransportService, TransportService>();

        #region Users
        services.AddScoped<IJobKindRepository, JobKindRepository>();
        services.AddScoped<IJobKindService, JobKindService>();
        #endregion

        #region Contacts
        services.AddScoped<ICallStatusRepository, CallStatusRepository>();
        services.AddScoped<ICallStatusService, CallStatusService>();
        #endregion

        services.AddScoped<IGenerator, LeafGenerator>();
        services.AddScoped<IGenerator, ComponentGenerator>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<IVoucherService, VoucherService>();
        services.AddScoped<IGiftRepository, GiftRepository>();
        services.AddScoped<IGiftService, GiftService>();

        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ISourceRepository, SourceRepository>();
        services.AddScoped<ISourceService, SourceService>();
        services.AddScoped<ICallCenterRepository, CallCenterRepository>();
        services.AddScoped<ICallCenterService, CallCenterService>();
        services.AddScoped<IGroupDataRepository, GroupDataRepository>();
        services.AddScoped<IGroupDataService, GroupDataService>();

        #region Finances
        services.AddScoped<IContractRepository, ContractRepository>();
        services.AddScoped<IContractService, ContractService>();
        services.AddScoped<ICouponRepository, CouponRepository>();
        services.AddScoped<ICouponService, CouponService>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IBillRepository, BillRepository>();
        services.AddScoped<IBillService, BillService>();
        #endregion
    }
}

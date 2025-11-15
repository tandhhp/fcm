using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Waffle.Entities;
using Waffle.Entities.Contacts;
using Waffle.Entities.Contracts;
using Waffle.Entities.Ecommerces;
using Waffle.Entities.Healthcares;
using Waffle.Entities.Payments;
using Waffle.Entities.Tours;
using Waffle.Entities.Users;

namespace Waffle.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    public DbSet<AppLog> AppLogs { get; set; }
    public DbSet<AppSetting> AppSettings { get; set; }
    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<FileContent> FileContents { get; set; }
    public DbSet<WorkContent> WorkContents { get; set; }
    public DbSet<WorkItem> WorkItems { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Localization> Localizations { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<TourCatalog> TourCatalogs { get; set; }
    public DbSet<Itinerary> Itineraries { get; set; }
    public DbSet<Form> Forms { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Healthcare> Healthcares { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<SubUser> SubUsers { get; set; }
    public DbSet<ContactActivity> ContactActivities { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<TourAmenity> TourAmenities { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<UserInfo> UserInfos { get; set; }
    public DbSet<UserTopup> UserTopups { get; set; }
    public DbSet<Lead> Leads { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<LeadFeedback> LeadFeedbacks { get; set; }
    public DbSet<SubLead> SubLeads { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<EventTable> EventTables { get; set; }
    public DbSet<LeadHistory> LeadHistories { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Gift> Gifts { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<JobKind> JobKinds { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Transport> Transports { get; set; }
    public DbSet<CallStatus> CallStatuses { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public virtual DbSet<CallHistory> CallHistories { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<ContractGift> ContractGifts { get; set; }
    public DbSet<CallCenter> CallCenters { get; set; }
    public DbSet<GroupData> GroupDatas { get; set; }

    #region Finances
    public DbSet<Coupon> Coupons { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<WorkItem>().HasKey(k => new { k.WorkId, k.CatalogId });
        builder.Entity<TourAmenity>().HasKey(k => new { k.CatalogId, k.AmenityId });
        builder.Entity<ContractGift>().HasKey(k => new { k.ContractId, k.GiftId });
    }
}
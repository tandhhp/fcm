using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.ExternalAPI.Interfaces;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.Pages
{
    public class IndexModel : EntryPageModel
    {
        private readonly IWordPressService _wordPressService;
        private readonly ApplicationDbContext _context;
        public IndexModel(ICatalogService catalogService, IWordPressService wordPressService, ApplicationDbContext context) : base(catalogService)
        {
            _wordPressService = wordPressService;
            _context = context;
        }

        public List<ComponentListItem> Components { get; set; } = new();

        public List<SwiperItem> HeroSlides { get; set; } = new()
        {
            new SwiperItem
            {
                Title = "Sứ mệnh",
                Image = "https://nuras.com.vn/videos/intro.mp4",
                IsVideo = true,
                Voucher = "https://nuras.com.vn/imgs/vouchers/voucher-medlatec.png",
                Description = "Xây dựng thói quen chăm sóc sức khỏe chủ động cho người Việt bằng những ứng dụng công nghệ y học tiên tiến vào đời sống với mục đích giúp người trưởng thành luôn khỏe mạnh, trẻ trung để tận hưởng cuộc sống hạnh phúc hơn."
            },
            new SwiperItem
            {
                Title = "Chăm sóc<br/>sức khỏe chủ động",
                Image = "https://nuras.com.vn/imgs/homes/asia.jpg",
                Type = HeroType.Healthcare,
                Voucher = "https://nuras.com.vn/imgs/vouchers/voucher-medlatec.png",
                Description = "NURA'S tập trung vào việc phòng ngừa bệnh tật, giúp khách hàng nâng cao sức khỏe và phòng tránh nguy cơ mắc bệnh. Đặc biệt, đến NURA'S sẽ luôn có trợ lí cá nhân đồng hành cùng khách hàng trong mọi khía cạnh sức khoẻ, từ Y tế - Dưỡng sinh, đặc quyền 1-1."
            },
            new SwiperItem
            {
                Title = "Kỳ nghỉ<br/>dưỡng sinh độc bản",
                Image = "https://nuras.com.vn/imgs/homes/tea.jpg",
                Voucher = "https://nuras.com.vn/imgs/vouchers/voucher-tour.png",
                Type = HeroType.Tour,
                Description = "Nuras cung cấp kỳ nghỉ dưỡng sinh độc bản. Khách hàng được trải nghiệm kỳ nghỉ tại các resort đẳng cấp đi kèm những dịch vụ tuyệt vời cho sức khoẻ như yoga, thiền chuông, thiền nến, onsen, spa, chăm sóc sức khoẻ…"
            },
            new SwiperItem
            {
                Title = "Công nghệ tế bào gốc",
                Voucher = "https://nuras.com.vn/imgs/vouchers/voucher-medlatec.png",
                Image = "https://nuras.com.vn/imgs/homes/te-bao-goc.jpg",
                Description = "NURA'S ứng dụng công nghệ tế bào gốc giúp phòng ngừa các bệnh lý và trẻ hóa cơ thể"
            }
        };

        public List<Catalog> Articles = new();
        public Guid? AchievementId;

        public async Task<IActionResult> OnGetAsync(Guid? achievementId)
        {
            Components = await _catalogService.ListComponentAsync(PageData.Id);

            if (achievementId != null)
            {
                var ach = await _context.Achievements.FirstOrDefaultAsync(x => x.Id == achievementId);
                if (ach != null)
                {
                    ViewData["Image"] = ach.Icon;
                    AchievementId = achievementId;
                }
            }

            var articles = await _wordPressService.ListPostAsync("https://nuras.vn/", new SearchFilterOptions
            {
                Current = 1,
                PageSize = 4
            });
            if (articles != null)
            {
                Articles = articles.Select(x => new Catalog
                {
                    Name = x.Title.Rendered ?? string.Empty,
                    Description = x.Excerpt.Rendered,
                    Active = true,
                    CreatedDate = x.Date ?? DateTime.Now,
                    ViewCount = x.Id,
                    Thumbnail = x.Embedded.FeaturedMedia.FirstOrDefault()?.MediaDetails.Sizes.MediumLarge.SourceUrl ?? x.Embedded.FeaturedMedia.FirstOrDefault()?.MediaDetails.Sizes.Thumbnail.SourceUrl,
                    NormalizedName = x.Link ?? string.Empty,
                }).ToList();
            }
            return Page();
        }
    }
}

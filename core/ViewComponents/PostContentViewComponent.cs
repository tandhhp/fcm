using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.ExternalAPI.Interfaces;
using Waffle.Models.Components.Specifications;

namespace Waffle.ViewComponents
{
    public class PostContentViewComponent : BaseViewComponent<PostContent>
    {
        private readonly IWordPressService _wordPressService;
        private readonly IGoogleService _googleService;
        private readonly ISettingService _appService;

        public PostContentViewComponent(IWorkService workService, IWordPressService wordPressService, IGoogleService googleService, ISettingService appService) : base(workService)
        {
            _wordPressService = wordPressService;
            _googleService = googleService;
            _appService = appService;
        }

        protected override async Task<PostContent> ExtendAsync(PostContent work)
        {
            switch (work.Type)
            {
                case PostContentType.BlockEditor:
                    ViewName = PostContentType.BlockEditor.ToString();
                    break;
                case PostContentType.WordPress:
                    var wordPress = await _wordPressService.GetPostAsync(work.WordPress.Domain, work.WordPress.Id.ToString());
                    work.Content = wordPress?.Content.Rendered ?? string.Empty;
                    break;
                case PostContentType.Blogspot:
                    var blogger = await _appService.GetAsync<ExternalAPI.GoogleAggregate.Google>(nameof(Google));
                    if (blogger != null)
                    {
                        var post = await _googleService.BloggerGetAsync(work.Blogger.BlogId, work.Blogger.PostId, blogger.BloggerApiKey);
                        work.Content = post?.Content ?? string.Empty;
                    }
                    break;
                default:
                    break;
            }
            return work;
        }
    }
}

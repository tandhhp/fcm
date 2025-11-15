using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Models.Components.Specifications;

namespace Waffle.ViewComponents
{
    public class VideoPlayerViewComponent : BaseViewComponent<VideoPlayer>
    {
        public VideoPlayerViewComponent(IWorkService workService) : base(workService)
        {
        }

        protected override Task<VideoPlayer> ExtendAsync(VideoPlayer work)
        {
            work.Description = PageData.Description;
            return base.ExtendAsync(work);
        }
    }
}

using Waffle.ExternalAPI.Game;
using Waffle.ExternalAPI.Interfaces;
using Waffle.ExternalAPI.Services;
using Waffle.ExternalAPI.Tel4vn;

namespace Waffle.Extensions;

public static class AddHttpClientExtensions
{
    public static void AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<IFacebookService, FacebookService>();
        services.AddHttpClient<IGameService, GameService>();
        services.AddHttpClient<IGoogleService, GoogleService>();
        services.AddHttpClient<IShopeeService, ShopeeService>();
        services.AddHttpClient<IWikiService, WikiService>();
        services.AddHttpClient<IWordPressService, WordPressService>();
        services.AddHttpClient<ITel4vnService, Tel4vnService>();
    }
}

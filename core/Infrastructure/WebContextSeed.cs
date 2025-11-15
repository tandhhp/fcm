namespace Waffle.Infrastructure;

public class WebContextSeed
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        var log = applicationBuilder.ApplicationServices.GetRequiredService<ILogger<WebContextSeed>>();
        log.LogTrace("Context Seed is runing!");
    }

}
